using Domain.Common;
using Domain.VirtualMachines.BackUp;
using Domain.VirtualMachines.Contract;
using Domain.Statistics;
using Domain.VirtualMachines.VirtualMachine;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using Shared.Servers;

namespace Shared.VirtualMachines;

public static class VirtualMachineDto
{
    public class Index
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public VirtualMachineMode Mode { get; set; }

    }
    public class Detail : Index
    {
        public Hardware Hardware { get; set; }
        public OperatingSystemEnum OperatingSystem { get; set; }
        public VMContract? Contract { get; set; }
        public Backup BackUp { get; set; }
        public FysiekeServerDto.Index FysiekeServer { get; set; }
        public VMConnectionDto.Detail? VMConnection { get; set; }
    }

    public class Rapportage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Statistic Statistics { get; set; }

    }
    public class Edit
    {
        [Required(ErrorMessage = "Waarde mag niet leeg zijn")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Waarde moet tussen 3 en 30 liggen.")]
        public String Name { get; set; }

        [Required(ErrorMessage = "Backuptype mag niet leeg zijn")]
        public Backup Backup { get; set; }
    }
    public class Mutate
    {
        [Required(ErrorMessage = "Je moet een naam ingeven voor de VM.")]
        [StringLength(30, ErrorMessage = "Naam is te lang.")]
        public String Name { get; set; }

        [Required(ErrorMessage = "Hardware moet volledig worden ingeven.")]
        public Hardware Hardware { get; set; }

        [Required(ErrorMessage = "Je moet een besturingssysteem kiezen.")]
        [EnumDataType(typeof(OperatingSystemEnum), ErrorMessage = "Ongeldige waarde")]
        public OperatingSystemEnum OperatingSystem { get; set; }

        [Required(ErrorMessage = "Je moet een backuptype kiezen.")]
        public Backup Backup { get; set; }

        [Required(ErrorMessage = "Je moet een project selecteren.")]
        public int ProjectId { get; set; }


        [Required(ErrorMessage = "Je moet een startdatum kiezen.")]
        public DateTime Start { get; set; }

        [Required(ErrorMessage = "Je moet een einddatum kiezen.")]
        public DateTime End { get; set; }




    }
    public class Validator : AbstractValidator<Mutate>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty().Length(5, 50);
                RuleFor(x => x.Hardware.Amount_vCPU).LessThan(50);
                RuleFor(x => x.Hardware.Storage).NotEmpty();
                RuleFor(x => x.Hardware.Memory).NotEmpty();
                RuleFor(x => x.ProjectId).NotNull();
                RuleFor(x => x.Start).NotEmpty();
                RuleFor(x => x.End).NotEmpty();
                RuleFor(x => x.Backup.Type).NotNull();

            }
        }
}