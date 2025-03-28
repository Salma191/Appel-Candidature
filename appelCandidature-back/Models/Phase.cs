using System.ComponentModel.DataAnnotations;

namespace pfe_back.Models
{
    public class Phase
    {
        public int Id { get; set; }
        public NomPhase Nom { get; set; }

        public ICollection<DecisionPhase>? DecisionPhases { get; set; }
    }

    public enum NomPhase
    {
        [Display(Name = "Décision en Draft")]
        Draft,
        [Display(Name = "Décision lancées")]
        Lancement,
        [Display(Name = "Vérification des candidatures")]
        Preselection,
        [Display(Name = "Entretiens")]
        Entretien,
        [Display(Name = "Evaluation")]
        Selection,
        [Display(Name = "Nomination")]
        Publication
    }
}
