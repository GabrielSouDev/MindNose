using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MindNose.Domain.Request;

public class LinksRequest
{
    private string _category {get;set;} = string.Empty;
    public string Category
    {
        get => _category;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _category = string.Empty;
            }
            else
            {
                _category = value.CategoryNormalize();
            }
        }
    }
    private string _term { get; set; } = string.Empty;
    public string Term
    {
        get => _term;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _term = string.Empty;
            }
            else
            {
                _term = value.TermNormalize();
            }
        }
    }
    public string LLMModel { get; set; } = "mistralai/mistral-small-3.2-24b-instruct";
    public int LengthPath { get; set; } = 1;
    public int Limit { get; set; } = 10;
    public int Skip { get; set; } = 0;
}
