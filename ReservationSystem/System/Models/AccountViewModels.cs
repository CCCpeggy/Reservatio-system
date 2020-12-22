using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace System.Models
{
    // AccountController 動作所傳回的模型。
    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }
}
