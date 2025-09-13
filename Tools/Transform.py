import os
import torch
from transformers import AutoTokenizer, AutoModel
import onnxruntime as ort
import numpy as np

# --- Configurações ---
model_dir = "./"  # caminho onde estão os arquivos do modelo
output_dir = "./e5-base-onnx"
onnx_output_path = os.path.join(output_dir, "model.onnx")

# --- Criar pasta de saída ---
os.makedirs(output_dir, exist_ok=True)

# --- Carregar modelo e tokenizer ---
tokenizer = AutoTokenizer.from_pretrained(model_dir)
model = AutoModel.from_pretrained(model_dir)
model.eval()

# --- Exportar para ONNX ---
dummy_input = torch.randint(0, 1000, (1, 16))  # batch=1, seq_len=16
attention_mask = torch.ones_like(dummy_input)

torch.onnx.export(
    model,
    (dummy_input, attention_mask),
    onnx_output_path,
    input_names=["input_ids", "attention_mask"],
    output_names=["last_hidden_state"],
    dynamic_axes={
        "input_ids": {0: "batch_size", 1: "seq_len"},
        "attention_mask": {0: "batch_size", 1: "seq_len"},
        "last_hidden_state": {0: "batch_size", 1: "seq_len"}
    },
    opset_version=14
)

print(f"Modelo ONNX salvo em {onnx_output_path}")

# --- Função para gerar embeddings usando ONNX ---
def get_embeddings(texts):
    inputs = tokenizer(texts, padding=True, truncation=True, return_tensors="np")
    ort_sess = ort.InferenceSession(onnx_output_path)
    ort_inputs = {k: v for k, v in inputs.items() if k in [inp.name for inp in ort_sess.get_inputs()]}
    outputs = ort_sess.run(None, ort_inputs)
    last_hidden_states = outputs[0]

    # mean pooling com atenção
    attention_mask = inputs["attention_mask"][..., None].astype(np.float32)
    last_hidden_states = last_hidden_states * attention_mask
    summed = last_hidden_states.sum(axis=1)
    lengths = attention_mask.sum(axis=1)
    embeddings = summed / np.where(lengths == 0, 1e-9, lengths)  # evita divisão por zero

    # normalizar embeddings
    norm = np.linalg.norm(embeddings, axis=1, keepdims=True)
    embeddings = embeddings / np.where(norm == 0, 1e-9, norm)
    return embeddings

# --- Exemplo de uso ---
texts = ["O que é a capital da França?", "Paris é a capital da França."]
embeddings = get_embeddings(texts)
print(embeddings.shape)  # deve retornar (2, 768)

