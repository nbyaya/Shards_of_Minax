using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a zel'vrak corpse")]
    public class ZelVrak : BaseCreature
    {
        private DateTime m_NextMysticVeil;
        private DateTime m_NextEnigmaticSurge;
        private DateTime m_NextVeiledIllusion;
        private bool m_IsVeiled;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public ZelVrak()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Zel'Vrak the Enigmatic";
            Body = 22; // ElderGazer body
            Hue = 1759; // Custom hue for Zel'Vrak
			BaseSoundID = 377;

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

            m_AbilitiesInitialized = false;
        }

        public ZelVrak(Serial serial)
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
                    Random rand = new Random();
                    m_NextMysticVeil = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextEnigmaticSurge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextVeiledIllusion = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextMysticVeil)
                {
                    MysticVeil();
                }

                if (DateTime.UtcNow >= m_NextEnigmaticSurge)
                {
                    EnigmaticSurge();
                }

                if (DateTime.UtcNow >= m_NextVeiledIllusion)
                {
                    VeiledIllusion();
                }
            }
        }

        private void MysticVeil()
        {
            if (!m_IsVeiled)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Mystic Veil Activated *");
                m_IsVeiled = true;
                SetResistance(ResistanceType.Physical, 70, 80);
                SetResistance(ResistanceType.Energy, 70, 80);

                Timer.DelayCall(TimeSpan.FromSeconds(30), new TimerCallback(EndMysticVeil));

                m_NextMysticVeil = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Update to a fixed cooldown after activation
            }
        }

        private void EndMysticVeil()
        {
            m_IsVeiled = false;
            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Energy, 60, 70);
        }

        private void EnigmaticSurge()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Enigmatic Surge! *");

            int elementalDamage = Utility.Random(4);

            if (Combatant != null && Combatant.Alive)
            {
                switch (elementalDamage)
                {
                    case 0:
                        Combatant.Damage(Utility.RandomMinMax(10, 20), this); // Fire damage
                        break;
                    case 1:
                        Combatant.Damage(Utility.RandomMinMax(10, 20), this); // Cold damage
                        break;
                    case 2:
                        Combatant.Damage(Utility.RandomMinMax(10, 20), this); // Poison damage
                        break;
                    case 3:
                        Combatant.Damage(Utility.RandomMinMax(10, 20), this); // Energy damage
                        break;
                }
            }

            m_NextEnigmaticSurge = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Update to a fixed cooldown after activation
        }

        private void VeiledIllusion()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Veiled Illusions *");
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 0x1F4, 0, 0, 0);

            for (int i = 0; i < 2; i++)
            {
                BaseCreature clone = new ZelVrakClone(this);
                clone.MoveToWorld(Location, Map);
                clone.Combatant = Combatant;

                Timer.DelayCall(TimeSpan.FromMinutes(1), new TimerCallback(delegate { if (!clone.Deleted) clone.Delete(); }));
            }

            m_NextVeiledIllusion = DateTime.UtcNow + TimeSpan.FromMinutes(3); // Update to a fixed cooldown after activation
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
            m_AbilitiesInitialized = false; // Reset the flag on deserialize
        }
    }

    public class ZelVrakClone : BaseCreature
    {
        private Mobile m_Master;

        public ZelVrakClone(Mobile master)
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            m_Master = master;
            Body = master.Body;
            Hue = master.Hue;
            Name = master.Name;

            SetStr(1);
            SetDex(1);
            SetInt(1);

            SetHits(1);
            SetDamage(0);

            SetResistance(ResistanceType.Physical, 100);
            SetResistance(ResistanceType.Fire, 100);
            SetResistance(ResistanceType.Cold, 100);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 100);

            VirtualArmor = 100;
        }

        public ZelVrakClone(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            if (m_Master == null || m_Master.Deleted)
            {
                Delete();
                return;
            }

            if (Combatant == null)
                Combatant = m_Master.Combatant;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_Master);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Master = reader.ReadMobile();
        }
    }
}
