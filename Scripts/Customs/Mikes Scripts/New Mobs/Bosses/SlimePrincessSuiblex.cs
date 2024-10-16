using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    public class SlimePrincessSuiblex : BaseCreature
    {
        private const int INITIAL_PHASE = 0;
        private const int PHASE_2 = 1;
        private const int PHASE_3 = 2;
        private const int FINAL_PHASE = 3;

        private int _currentPhase;
        private DateTime _nextAbilityTime;

        [Constructable]
        public SlimePrincessSuiblex() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Slime Princess Suiblex";
            Body = 16; // You may want to use a body type that resembles a slime or gooey creature
            BaseSoundID = 440; // Adjust as needed for a gooey sound

            SetStr(800, 1000);
            SetDex(150, 200);
            SetInt(200, 300);

            SetHits(1200);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Poison, 30);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 120.1, 130.0);
            SetSkill(SkillName.Magery, 120.1, 130.0);
            SetSkill(SkillName.MagicResist, 150.5, 200.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Wrestling, 90.0, 100.0);

            Fame = 50000;
            Karma = -50000;

            _currentPhase = INITIAL_PHASE;
            _nextAbilityTime = DateTime.Now;
        }

        public SlimePrincessSuiblex(Serial serial) : base(serial)
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
                Say("Feel the full force of my slime!");
                PerformPhaseTransition();
            }
            else if (healthPercentage <= 50 && _currentPhase < PHASE_3)
            {
                _currentPhase = PHASE_3;
                Say("My power grows stronger!");
                PerformPhaseTransition();
            }
            else if (healthPercentage <= 75 && _currentPhase < PHASE_2)
            {
                _currentPhase = PHASE_2;
                Say("Prepare for my gooey wrath!");
                PerformPhaseTransition();
            }
        }

        private void PerformPhaseTransition()
        {
            switch (_currentPhase)
            {
                case PHASE_2:
                    SlimeFlood();
                    break;
                case PHASE_3:
                    ToxicSpit();
                    break;
                case FINAL_PHASE:
                    AcidicRain();
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
                    SlimeSpray();
                    break;
                case 1:
                    GooBall();
                    break;
                case 2:
                    StickyTrap();
                    break;
            }
        }

        private void UsePhase2Ability()
        {
            if (Utility.RandomBool())
                SlimeFlood();
            else
                SlimySummon();
        }

        private void UsePhase3Ability()
        {
            if (Utility.RandomBool())
                ToxicSpit();
            else
                AcidicLash();
        }

        private void UseFinalPhaseAbility()
        {
            switch (Utility.Random(5))
            {
                case 0:
                    SlimeSpray();
                    break;
                case 1:
                    GooBall();
                    break;
                case 2:
                    SlimeFlood();
                    break;
                case 3:
                    ToxicSpit();
                    break;
                case 4:
                    AcidicRain();
                    break;
            }
        }

        private void SlimeSpray()
        {
            Say("Feel my Slime Spray!");

            // Slime spray effect
            Effects.SendLocationEffect(this.Location, this.Map, 0x3730, 30, 10); // Green slime visual effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m is PlayerMobile && m != this)
                {
                    m.SendMessage("You are hit by a gooey slime spray!");
                    m.Damage(Utility.RandomMinMax(20, 30), this);
                }
            }
        }

        private void GooBall()
        {
            Say("Take this Goo Ball!");

            // Create goo ball effect
            Point3D targetLocation = GetRandomLocationInRange(10);
            Effects.SendLocationEffect(targetLocation, this.Map, 0x3730, 30, 10);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m is PlayerMobile && m.GetDistanceToSqrt(targetLocation) <= 2)
                {
                    m.SendMessage("You are hit by a goo ball!");
                    m.Damage(Utility.RandomMinMax(15, 25), this);
                }
            }
        }

        private void StickyTrap()
        {
            Say("Beware of my Sticky Trap!");

            // Create sticky trap effect
            Point3D targetLocation = GetRandomLocationInRange(5);
            Effects.SendLocationEffect(targetLocation, this.Map, 0x3730, 30, 10);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m is PlayerMobile && m.GetDistanceToSqrt(targetLocation) <= 2)
                {
                    m.SendMessage("You are stuck in a sticky trap!");
                    m.Damage(Utility.RandomMinMax(10, 20), this);
                }
            }
        }

        private void SlimeFlood()
        {
            Say("The ground is flooded with slime!");

            // Create slime flood effect
            Point3D floodLocation = GetRandomLocationInRange(5);
            for (int i = 0; i < 5; i++)
            {
                Effects.SendLocationEffect(floodLocation, this.Map, 0x3730, 30, 10);
                floodLocation.X += Utility.RandomMinMax(-3, 3);
                floodLocation.Y += Utility.RandomMinMax(-3, 3);
            }

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m is PlayerMobile)
                {
                    m.SendMessage("You are engulfed in slime!");
                    m.Damage(Utility.RandomMinMax(20, 35), this);
                }
            }
        }

        private void SlimySummon()
        {
            Say("I summon my slimy minions!");

            // Summon slimy minions
            for (int i = 0; i < 3; i++)
            {
                BaseCreature slime = new Slime(); // Replace with your slime minion type
                slime.MoveToWorld(GetRandomLocationInRange(10), this.Map);
                slime.FightMode = FightMode.Closest;
                slime.AI = AIType.AI_Mage;
            }
        }

        private void ToxicSpit()
        {
            Say("Taste my Toxic Spit!");

            // Create toxic spit effect
            Point3D targetLocation = GetRandomLocationInRange(10);
            Effects.SendLocationEffect(targetLocation, this.Map, 0x3730, 30, 10);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m is PlayerMobile && m.GetDistanceToSqrt(targetLocation) <= 2)
                {
                    m.SendMessage("You are hit by toxic spit!");
                    m.Damage(Utility.RandomMinMax(25, 40), this);
                }
            }
        }

        private void AcidicLash()
        {
            Say("Feel my Acidic Lash!");

            // Create acidic lash effect
            for (int i = 0; i < 3; i++)
            {
                Point3D lashLocation = GetRandomLocationInRange(5);
                Effects.SendLocationEffect(lashLocation, this.Map, 0x3730, 30, 10);
            }

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m is PlayerMobile)
                {
                    m.SendMessage("You are hit by an acidic lash!");
                    m.Damage(Utility.RandomMinMax(20, 30), this);
                }
            }
        }

        private void AcidicRain()
        {
            Say("Acidic Rain falls upon you!");

            // Create acidic rain effect
            Point3D rainLocation = this.Location;
            Effects.SendLocationEffect(rainLocation, this.Map, 0x3730, 30, 10);

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m is PlayerMobile)
                {
                    m.SendMessage("You are caught in the acidic rain!");
                    m.Damage(Utility.RandomMinMax(35, 50), this);
                }
            }
        }

        private Point3D GetRandomLocationInRange(int range)
        {
            int x = this.X + Utility.RandomMinMax(-range, range);
            int y = this.Y + Utility.RandomMinMax(-range, range);
            return new Point3D(x, y, this.Z);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(_currentPhase);
            writer.Write(_nextAbilityTime);
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
