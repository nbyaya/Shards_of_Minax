using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a wraith lich's corpse")]
    public class WraithLich : BaseCreature
    {
        private DateTime m_NextEtherealForm;
        private DateTime m_NextSpectralSiphon;
        private DateTime m_NextGhostlyApparition;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public WraithLich()
            : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a wraith lich";
            Body = 24; // Lich body
            Hue = 2131; // Unique spectral hue
            BaseSoundID = 0x3E9;

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

        public WraithLich(Serial serial)
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
                    m_NextEtherealForm = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSpectralSiphon = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextGhostlyApparition = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextEtherealForm)
                {
                    UseEtherealForm();
                }

                if (DateTime.UtcNow >= m_NextSpectralSiphon)
                {
                    UseSpectralSiphon();
                }

                if (DateTime.UtcNow >= m_NextGhostlyApparition)
                {
                    SummonGhostlyApparition();
                }
            }
        }

        private void UseEtherealForm()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Wraith Lich becomes ethereal! *");
            PlaySound(0x20B);
            FixedEffect(0x3728, 10, 30);

            // Ethereal Form: Temporarily becomes intangible (just an effect here)
            Timer.DelayCall(TimeSpan.FromSeconds(5), () => FixedEffect(0x3728, 10, 0)); // End ethereal form effect

            m_NextEtherealForm = DateTime.UtcNow + TimeSpan.FromMinutes(2);
        }

        private void UseSpectralSiphon()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                int manaDrain = Utility.RandomMinMax(10, 20);
                target.Mana -= manaDrain;
                Mana += manaDrain;
                target.SendMessage("The Wraith Lich drains your mana!");
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Spectral Siphon! *");
            }

            m_NextSpectralSiphon = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        private void SummonGhostlyApparition()
        {
            Point3D loc = GetSpawnPosition(2);

            if (loc != Point3D.Zero)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A ghostly apparition appears! *");
                PlaySound(0x20D);
                FixedEffect(0x3709, 10, 16);

                GhostlyApparition ghost = new GhostlyApparition();
                ghost.MoveToWorld(loc, Map);

                Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
                {
                    if (!ghost.Deleted)
                        ghost.Delete();
                });

                m_NextGhostlyApparition = DateTime.UtcNow + TimeSpan.FromMinutes(2);
            }
        }

        private Point3D GetSpawnPosition(int range)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-range, range);
                int y = Y + Utility.RandomMinMax(-range, range);
                int z = Map.GetAverageZ(x, y);

                Point3D p = new Point3D(x, y, z);

                if (Map.CanSpawnMobile(p))
                    return p;
            }

            return Point3D.Zero;
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

    public class GhostlyApparition : BaseCreature
    {
        public GhostlyApparition()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a ghostly apparition";
            Body = 24;
            Hue = 1155; // Ghostly hue
            BaseSoundID = 0x482;

            SetStr(50, 60);
            SetDex(40, 50);
            SetInt(80, 100);

            SetHits(30, 40);
            SetDamage(5, 10);

            SetDamageType(ResistanceType.Cold, 100);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.EvalInt, 60.0);
            SetSkill(SkillName.Magery, 60.0);
            SetSkill(SkillName.Meditation, 60.0);
            SetSkill(SkillName.MagicResist, 60.0);

            VirtualArmor = 20;
        }

        public GhostlyApparition(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                // Ghostly Apparition combat logic can be added here
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
        }
    }
}
