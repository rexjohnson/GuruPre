using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuruETC.Web.Models
{
    public class QuestionPart
    {
        public List<QuestionProp> QuestionPart1 { get; set; }
        public List<QuestionProp> QuestionPart2 { get; set; }
        public List<QuestionProp> QuestionPart3 { get; set; }
        public List<QuestionProp> QuestionPart4 { get; set; }
        public List<QuestionProp> QuestionPart5 { get; set; }
        public List<QuestionProp> QuestionPart6 { get; set; }
        public List<QuestionProp> QuestionPart7 { get; set; }

        public List<CategoryProp> CategoryPart1 { get; set; }

    }

    public class CategoryProp
    {
        public long? CatId { get; set; }
        public string CName { get; set; }
    }

    public class QuestionProp
    {
        public long? CatId { get; set; }
        public long? QId { get; set; }
        public string QName { get; set; }
        public string CName { get; set; }
        public string QValue { get; set; }
        public string QClass { get; set; }
    }


    

}