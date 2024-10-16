using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an ember serpent corpse")]
    public class EmberSerpent : BaseCreature
    {
        private DateTime m_NextEmberTrail;
        private DateTime m_NextScorchedBite;
        private DateTime m_NextFlameCoil;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public EmberSerpent()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "an ember serpent";
            this.Body = 15; // Fire elemental body
            this.BaseSoundID = 838;
            this.Hue = 1661; // Unique hue for the ember serpent

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

        public EmberSerpent(Serial serial)
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

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextEmberTrail = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextScorchedBite = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextFlameCoil = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextEmberTrail)
                {
                    UseEmberTrail();
                }

                if (DateTime.UtcNow >= m_NextScorchedBite)
                {
                    UseScorchedBite();
                }

                if (DateTime.UtcNow >= m_NextFlameCoil)
                {
                    UseFlameCoil();
                }
            }
        }

        private void UseEmberTrail()
        {
            foreach (Mobile m in GetMobilesInRange(1))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are scorched by the ember trail left by the serpent!");
                    m.Damage(10, this);
                }
            }

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The ember serpent leaves a burning trail behind it! *");
            m_NextEmberTrail = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Set cooldown after use
        }

        private void UseScorchedBite()
        {
            Mobile target = Combatant as Mobile; // Explicit cast
            if (target != null && target.Alive)
            {
                target.Damage(20, this);
                target.SendMessage("You feel the searing pain of the ember serpent's fiery bite!");
                target.Stam -= 20;

                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The ember serpent bites with fiery intensity! *");
                m_NextScorchedBite = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Set cooldown after use
            }
        }

        private void UseFlameCoil()
        {
            Mobile target = Combatant as Mobile; // Explicit cast
            if (target != null && target.Alive)
            {
                for (int i = 0; i < 5; i++)
                {
                    target.Damage(15, this);
                    target.SendMessage("You are engulfed in flames as the ember serpent coils around you!");
                    target.SendMessage("Your movement is hindered by the intense heat!");
                    target.Dex -= 10;

                    Timer.DelayCall(TimeSpan.FromSeconds(1), () => { });
                }

                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The ember serpent coils around its prey, burning with fierce heat! *");
                m_NextFlameCoil = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Set cooldown after use
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
