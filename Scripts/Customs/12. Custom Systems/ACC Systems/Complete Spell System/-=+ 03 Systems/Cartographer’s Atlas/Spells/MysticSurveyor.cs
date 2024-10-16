using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.ACC.CSS.Systems.CartographyMagic
{
    public class MysticSurveyor : CartographySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Mystic Surveyor", "Surveyorum",
            21004,
            9300
        );

        private DateTime _lastCastTime; // To keep track of the last cast time
        private readonly TimeSpan _cooldown = TimeSpan.FromMinutes(10); // Cooldown period

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 90.0; } }
        public override int RequiredMana { get { return 50; } }

        public MysticSurveyor(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
            _lastCastTime = DateTime.MinValue; // Initialize to the earliest possible time
        }

        public override void OnCast()
        {
            // Check if the cooldown has elapsed
            if (DateTime.Now < _lastCastTime + _cooldown)
            {
                Caster.SendMessage("You must wait longer before using this spell again.");
                return;
            }

            if (CheckSequence())
            {
                // Play visual effects and sound
                Caster.FixedParticles(0x374A, 10, 15, 5037, EffectLayer.Head); // Mystic-looking effect around the head
                Caster.PlaySound(0x1FB); // A mystical sound

                // Determine what item to spawn
                double chance = Utility.RandomDouble();

                Item report;

                if (chance <= 0.10)
                {
                    // 10% chance to spawn a random special report
                    Type[] specialReports = new Type[]
                    {
                        typeof(AdvancedReport),
                        typeof(ExceptionalReport),
                        typeof(UrbanReport),
                        typeof(ArchaeologicalReport)
                    };
                    Type chosenReport = specialReports[Utility.Random(specialReports.Length)];
                    report = (Item)Activator.CreateInstance(chosenReport);
                }
                else
                {
                    // 90% chance to spawn a ScoutingReport
                    report = new ScoutingReport();
                }

                // Add the item to the caster's backpack
                if (report != null)
                {
                    Caster.Backpack.DropItem(report);
                    Caster.SendMessage("A report has been added to your backpack.");
                }

                _lastCastTime = DateTime.Now; // Update the last cast time
            }

            FinishSequence();
        }
    }
}
