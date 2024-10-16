using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an inferno dragon corpse")]
    public class InfernoDrakon : BaseCreature
    {
        private DateTime m_NextInfernoBreath;
        private DateTime m_NextFirestorm;
        private DateTime m_NextMoltenArmor;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public InfernoDrakon()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an inferno dragon";
            Body = 12; // Standard dragon body
            Hue = 1480; // Fiery hue
            BaseSoundID = 362;

            SetStr(1000, 1200);
            SetDex(177, 255);
            SetInt(151, 250);
			
            SetHits(700, 1200);
			
            SetDamage(29, 35);
			
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 65, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;
			
            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 93.9;

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public InfernoDrakon(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 5;
		public override bool CanAngerOnTame => true;
		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}	

        public override int Meat { get { return 19; } }
        public override int DragonBlood { get { return 8; } }
        public override int Hides { get { return 20; } }
        public override HideType HideType { get { return HideType.Barbed; } }
        public override int Scales { get { return 7; } }
        public override ScaleType ScaleType { get { return ScaleType.Red; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextInfernoBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextFirestorm = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextMoltenArmor = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextInfernoBreath)
                {
                    InfernoBreath();
                }

                if (DateTime.UtcNow >= m_NextFirestorm)
                {
                    Firestorm();
                }

                if (DateTime.UtcNow >= m_NextMoltenArmor)
                {
                    MoltenArmor();
                }
            }
        }

        private void InfernoBreath()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;

                if (target != null)
                {
                    target.SendMessage("You are engulfed in a searing inferno!");
                    target.Damage(30, this);
                    target.ApplyPoison(this, Poison.Greater);

                    Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 2115);

                    m_NextInfernoBreath = DateTime.UtcNow + TimeSpan.FromSeconds(30);
                }
            }

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Inferno Dragon breathes a torrent of flames! *");
        }

        private void Firestorm()
        {
            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("A firestorm engulfs you!");
                    m.Damage(20, this);
                }
            }

            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 2115);

            m_NextFirestorm = DateTime.UtcNow + TimeSpan.FromMinutes(2);

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Inferno Dragon calls down a firestorm! *");
        }

        private void MoltenArmor()
        {
            foreach (Mobile m in GetMobilesInRange(1))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are burned by the Inferno Dragon's molten armor!");
                    m.Damage(10, this);
                }
            }

            m_NextMoltenArmor = DateTime.UtcNow + TimeSpan.FromMinutes(1);

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Inferno Dragon's molten armor burns all who come close! *");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
