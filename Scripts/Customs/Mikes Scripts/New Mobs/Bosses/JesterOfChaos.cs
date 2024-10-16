using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    public class JesterOfChaos : BaseCreature
    {
        private const int INITIAL_PHASE = 0;
        private const int PHASE_2 = 1;
        private const int PHASE_3 = 2;
        private const int FINAL_PHASE = 3;

        private int _currentPhase;
        private DateTime _nextAbilityTime;

        [Constructable]
        public JesterOfChaos() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Jester of Chaos";
            Body = 401; // Set to a jester-like body type
            BaseSoundID = 0x1A8;

            SetStr(900, 1200);
            SetDex(250, 300);
            SetInt(200, 250);

            SetHits(1200);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Poison, 60);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 120.1, 130.0);
            SetSkill(SkillName.Magery, 120.1, 130.0);
            SetSkill(SkillName.MagicResist, 160.0, 200.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 100.0, 110.0);

            Fame = 60000;
            Karma = -60000;

            _currentPhase = INITIAL_PHASE;
            _nextAbilityTime = DateTime.Now;
        }

        public JesterOfChaos(Serial serial) : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null)
                return;

            if (DateTime.Now >= _nextAbilityTime)
            {
                UseAbility();
                _nextAbilityTime = DateTime.Now.AddSeconds(Utility.RandomMinMax(5, 15));
            }

            CheckPhaseTransition();
        }

        private void CheckPhaseTransition()
        {
            int healthPercentage = (Hits * 100) / HitsMax;

            if (healthPercentage <= 25 && _currentPhase != FINAL_PHASE)
            {
                _currentPhase = FINAL_PHASE;
                Say("Prepare for the final act!");
                PerformPhaseTransition();
            }
            else if (healthPercentage <= 50 && _currentPhase < PHASE_3)
            {
                _currentPhase = PHASE_3;
                Say("Let the chaos reign!");
                PerformPhaseTransition();
            }
            else if (healthPercentage <= 75 && _currentPhase < PHASE_2)
            {
                _currentPhase = PHASE_2;
                Say("The jesters’ tricks begin!");
                PerformPhaseTransition();
            }
        }

        private void PerformPhaseTransition()
        {
            switch (_currentPhase)
            {
                case PHASE_2:
                    SummonIllusions();
                    break;
                case PHASE_3:
                    JestersTrick();
                    break;
                case FINAL_PHASE:
                    FinalChaos();
                    break;
            }
        }

        private void UseAbility()
        {
            switch (_currentPhase)
            {
                case INITIAL_PHASE:
                    UseInitialPhaseAbility();
                    break;
                case PHASE_2:
                    UsePhase2Ability();
                    break;
                case PHASE_3:
                    UsePhase3Ability();
                    break;
                case FINAL_PHASE:
                    UseFinalPhaseAbility();
                    break;
            }
        }

        private void UseInitialPhaseAbility()
        {
            switch (Utility.Random(3))
            {
                case 0:
                    ConfusionSpell();
                    break;
                case 1:
                    HarlequinBlades();
                    break;
                case 2:
                    PranksterFireworks();
                    break;
            }
        }

        private void UsePhase2Ability()
        {
            if (Utility.RandomBool())
                MirrorIllusions();
            else
                MirrorIllusions();
        }

        private void UsePhase3Ability()
        {
            if (Utility.RandomBool())
                JestersTrick();
            else
                ChaoticHaste();
        }

        private void UseFinalPhaseAbility()
        {
            switch (Utility.Random(4))
            {
                case 0:
                    ConfusionSpell();
                    break;
                case 1:
                    MirrorIllusions();
                    break;
                case 2:
                    MirrorIllusions();
                    break;
                case 3:
                    ChaosStorm();
                    break;
            }
        }

        private void ConfusionSpell()
        {
            Say("Confusion reigns!");

            foreach (Mobile mobile in GetMobilesInRange(10))
            {
                if (mobile is PlayerMobile && mobile != this)
                {
                    mobile.SendMessage("You feel disoriented!");
                    mobile.SendLocalizedMessage(1060184); // You are disoriented!
                    mobile.Paralyze(TimeSpan.FromSeconds(5));
                }
            }
        }

        private void HarlequinBlades()
        {
            Say("Dance with my blades!");

            for (int i = 0; i < 5; i++)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.5), new TimerCallback(CreateHarlequinBlade));
            }
        }

        private void CreateHarlequinBlade()
        {
            Point3D location = new Point3D(Location.X + Utility.RandomMinMax(-5, 5), Location.Y + Utility.RandomMinMax(-5, 5), Location.Z);
            Item blade = new BlackPearl(); // Use a simple item for demonstration
            blade.MoveToWorld(location, Map);

            Effects.SendLocationEffect(location, Map, 0x36D4, 30, 10, 1153, 0); // Create a visual effect
        }

        private void PranksterFireworks()
        {
            Say("Enjoy the fireworks!");

            for (int i = 0; i < 3; i++)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(i * 1.5), new TimerCallback(LaunchFireworks));
            }
        }

        private void LaunchFireworks()
        {
            Point3D location = new Point3D(Location.X + Utility.RandomMinMax(-5, 5), Location.Y + Utility.RandomMinMax(-5, 5), Location.Z + 10);
            Effects.SendLocationEffect(location, Map, 0x36BD, 30, 10, 1153, 0); // Fireworks effect
        }

        private void SummonIllusions()
        {
            Say("Let the illusions begin!");

            for (int i = 0; i < 3; i++)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(i * 2), new TimerCallback(CreateIllusion));
            }
        }

        private void CreateIllusion()
        {
            BaseCreature illusion = new ShadowWyrm(); // Use a simple creature for demonstration
            illusion.MoveToWorld(new Point3D(Location.X + Utility.RandomMinMax(-5, 5), Location.Y + Utility.RandomMinMax(-5, 5), Location.Z), Map);
            illusion.Say("Beware of my illusions!");
        }

        private void MirrorIllusions()
        {
            Say("Behold my mirror images!");

            // Implement logic for mirror illusions
            for (int i = 0; i < 3; i++)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(i * 2), new TimerCallback(CreateMirrorImage));
            }
        }

        private void CreateMirrorImage()
        {
            BaseCreature mirrorImage = new ShadowWyrm(); // Use a simple creature for demonstration
            mirrorImage.MoveToWorld(new Point3D(Location.X + Utility.RandomMinMax(-5, 5), Location.Y + Utility.RandomMinMax(-5, 5), Location.Z), Map);
            mirrorImage.Say("I am everywhere!");
        }

        private void JestersTrick()
        {
            Say("Let’s play a trick!");

            // Implement a jester’s trick
            Point3D location = new Point3D(Location.X + Utility.RandomMinMax(-10, 10), Location.Y + Utility.RandomMinMax(-10, 10), Location.Z);
            Effects.SendLocationEffect(location, Map, 0x36D4, 30, 10, 1153, 0); // Chaos effect
        }

        private void ChaoticHaste()
        {
            Say("Feel the haste of chaos!");

        }

        private void FinalChaos()
        {
            Say("Embrace the chaos!");

            // Implement final chaotic ability
            Point3D location = new Point3D(Location.X + Utility.RandomMinMax(-10, 10), Location.Y + Utility.RandomMinMax(-10, 10), Location.Z);
            Effects.SendLocationEffect(location, Map, 0x36D4, 30, 10, 1153, 0); // Chaos storm effect
        }

        private void ChaosStorm()
        {
            Say("Unleashing chaos storm!");

            foreach (Mobile mobile in GetMobilesInRange(10))
            {
                if (mobile is PlayerMobile)
                {
                    mobile.Damage(Utility.RandomMinMax(30, 50), this);
                    mobile.SendMessage("You are caught in a storm of chaos!");
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write((int)_currentPhase);
            writer.Write((DateTime)_nextAbilityTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            _currentPhase = reader.ReadInt();
            _nextAbilityTime = reader.ReadDateTime();
        }
    }
}
