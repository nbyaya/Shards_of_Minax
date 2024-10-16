using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a Seti the Avenger corpse")]
    public class SetiTheAvenger : BaseCreature
    {
        private DateTime m_NextRengefulSpirits;
        private DateTime m_NextJudgmentOfTheGods;
        private DateTime m_NextCurseOfThePharaohs;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public SetiTheAvenger()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Seti the Avenger";
            Body = 154; // Mummy body
            Hue = 2160; // Unique hue
			BaseSoundID = 471;

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

        public SetiTheAvenger(Serial serial)
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
                    m_NextRengefulSpirits = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextJudgmentOfTheGods = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 30));
                    m_NextCurseOfThePharaohs = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 40));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextRengefulSpirits)
                {
                    CastRengefulSpirits();
                }

                if (DateTime.UtcNow >= m_NextJudgmentOfTheGods)
                {
                    CastJudgmentOfTheGods();
                }

                if (DateTime.UtcNow >= m_NextCurseOfThePharaohs)
                {
                    CastCurseOfThePharaohs();
                }
            }
        }

        private void CastRengefulSpirits()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Vengeful spirits rise from the ground to seek vengeance! *");
            PlaySound(0x1F2); // Ghostly sound

            for (int i = 0; i < 3; i++)
            {
                RengefulSpirit spirit = new RengefulSpirit();
                Point3D spawnLocation = new Point3D(Location.X + Utility.RandomMinMax(-5, 5), Location.Y + Utility.RandomMinMax(-5, 5), Location.Z);
                spirit.MoveToWorld(spawnLocation, Map);
                spirit.AggressivelyAttack(Combatant as Mobile);
            }

            m_NextRengefulSpirits = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void CastJudgmentOfTheGods()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Seti calls upon the gods to deliver judgment! *");
            PlaySound(0x1F3); // Spell sound

            if (Combatant != null && Combatant.Alive)
            {
                AOS.Damage(Combatant, this, Utility.RandomMinMax(40, 60), 0, 0, 100, 0, 0); // High damage
            }

            m_NextJudgmentOfTheGods = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void CastCurseOfThePharaohs()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Seti casts the Curse of the Pharaohs! *");
            PlaySound(0x1F4); // Cursed sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    int damage = Utility.RandomMinMax(15, 25);
                    AOS.Damage(m, this, damage, 0, 0, 100, 0, 0); // Damage
                    m.SendMessage("You feel an ancient curse weakening you!");
                    m.ApplyPoison(this, Poison.Lethal); // Apply curse poison
                    m.FixedParticles(0x374A, 10, 30, 5052, EffectLayer.Waist);
                }
            }

            m_NextCurseOfThePharaohs = DateTime.UtcNow + TimeSpan.FromSeconds(45);
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

            m_AbilitiesInitialized = false;
        }
    }

    public class RengefulSpirit : BaseCreature
    {
        [Constructable]
        public RengefulSpirit()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a vengeful spirit";
            Body = 154; // Ghost body
            Hue = 1154; // Ghostly hue

            SetStr(120);
            SetDex(120);
            SetInt(120);

            SetHits(80);

            SetDamage(15, 20);

            SetResistance(ResistanceType.Physical, 0);
            SetResistance(ResistanceType.Fire, 60);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 0);
            SetResistance(ResistanceType.Energy, 60);

            SetSkill(SkillName.MagicResist, 70.0, 90.0);
            SetSkill(SkillName.Tactics, 60.0, 80.0);
            SetSkill(SkillName.Wrestling, 60.0, 80.0);

            Fame = 3000;
            Karma = -3000;

            VirtualArmor = 30;
        }

        public RengefulSpirit(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                // Attack the combatant with ghostly damage
                AOS.Damage(Combatant, this, Utility.RandomMinMax(10, 20), 0, 0, 100, 0, 0);
            }
        }

        public void AggressivelyAttack(Mobile target)
        {
            Combatant = target;
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
        }
    }
}
