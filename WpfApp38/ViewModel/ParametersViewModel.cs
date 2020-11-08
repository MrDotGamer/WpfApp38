using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApp38.Models;

namespace WpfApp38.ViewModel
{
    public class ParametersViewModel
    {
        public string SourceId { get; set; }
        public string TargetId { get; set; }
        public string SourceValue { get; set; }
        public string TargetValue { get; set; }
        public string Status { get; set; }
         public static List<ParametersViewModel> AddedResultList(List<Parametras> sources,List<Parametras> targets)
        {
            var added = from t in targets
                        join s in sources
                        on t.Id equals s.Id into tempTarget
                        from tT in tempTarget.DefaultIfEmpty()
                        where tT == null
                        select new ParametersViewModel { TargetId = t.Id, SourceId = "", TargetValue = t.Value, SourceValue = "", Status = Const.Added };
            return added.ToList();
        }
        public static List<ParametersViewModel> RemovedResultList(List<Parametras> sources, List<Parametras> targets)
        {
            var removed = from s in sources
                          join t in targets
                          on s.Id equals t.Id into tempSources
                          from tS in tempSources.DefaultIfEmpty()
                          where tS == null
                          select new ParametersViewModel { SourceId = s.Id, SourceValue = s.Value, TargetId = "", TargetValue = "", Status = Const.Removed };
            return removed.ToList();
        }
        public static List<ParametersViewModel> ModifiedResultList(List<Parametras> sources, List<Parametras> targets)
        {
            var modified = from s in sources
                           join t in targets
                           on s.Id equals t.Id
                           where s.Value != t.Value
                           select new ParametersViewModel { SourceId = s.Id, SourceValue = s.Value, TargetId = t.Id, TargetValue = t.Value, Status = Const.Modified };
            return modified.ToList();
        }
        public static List<ParametersViewModel> UnchangedResultList(List<Parametras> sources, List<Parametras> targets)
        {
            var unchanged = from s in sources
                            join t in targets
                            on s.Id equals t.Id
                            where s.Value == t.Value
                            select new ParametersViewModel { SourceId = s.Id, SourceValue = s.Value, TargetId = t.Id, TargetValue = t.Value, Status = Const.Unchanged };
            return unchanged.ToList();
        }
    }
}
