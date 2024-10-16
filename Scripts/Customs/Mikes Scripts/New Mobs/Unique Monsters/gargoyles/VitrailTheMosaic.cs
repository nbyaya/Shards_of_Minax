using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a vitrail the mosaic corpse")]
    public class VitrailTheMosaic : BaseCreature
    {
        private DateTime m_NextPrismaticShatter;
        private DateTime m_NextLightRefract;
        private DateTime m_NextRadiantAura;
        private DateTime m_NextSummonMinions;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public VitrailTheMosaic()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Vitrail the Mosaic";
            Body = 4; // Gargoyle body
            Hue = 1665; // Unique hue for a stained-glass look
            BaseSoundID = 372;

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

        public VitrailTheMosaic(Serial serial)
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
                DateTime now = DateTime.UtcNow;

                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextPrismaticShatter = now + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextLightRefract = now + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextRadiantAura = now + TimeSpan.FromMinutes(rand.Next(1, 3));
                    m_NextSummonMinions = now + TimeSpan.FromMinutes(rand.Next(1, 3));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (now >= m_NextPrismaticShatter)
                {
                    PrismaticShatter();
                    m_NextPrismaticShatter = now + TimeSpan.FromSeconds(30); // Resetting cooldown
                }

                if (now >= m_NextLightRefract)
                {
                    LightRefract();
                    m_NextLightRefract = now + TimeSpan.FromSeconds(45); // Resetting cooldown
                }

                if (now >= m_NextRadiantAura)
                {
                    RadiantAura();
                    m_NextRadiantAura = now + TimeSpan.FromMinutes(2); // Resetting cooldown
                }

                if (now >= m_NextSummonMinions)
                {
                    SummonMinions();
                    m_NextSummonMinions = now + TimeSpan.FromMinutes(2); // Resetting cooldown
                }
            }
        }

        private void PrismaticShatter()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Vitrail shatters into dazzling fragments! *");
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x37F0, 10, 30, 0x21, 0, 5020, 0);
            
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player && m.Alive)
                {
                    m.Damage(40, this);
                }
            }

            Timer.DelayCall(TimeSpan.FromSeconds(5), new TimerCallback(Reassemble));
        }

        private void Reassemble()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Vitrail reassembles from the shards! *");
        }

        private void LightRefract()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Vitrail reflects the spell back with a radiant flare! *");
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Spell != null)
                {
                    Mobile caster = m;
                    caster.SendMessage("Your spell is reflected with increased power!");
                    caster.MagicDamageAbsorb = 0; // Remove any magic damage absorption
                    caster.SendMessage("Your spell is reflected with increased power!");
                }
            }
        }

        private void RadiantAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Vitrail's radiant aura shimmers brightly! *");
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x376A, 10, 16, 0x21, 0, 5020, 0);

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("The radiant aura reduces your accuracy!");
                    m.SendMessage("You feel your aim slipping!");
                }
            }
        }

        private void SummonMinions()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Vitrail summons minions to assist in battle! *");
            for (int i = 0; i < 2; i++)
            {
                MinionGargoyle minion = new MinionGargoyle();
                Point3D loc = new Point3D(X + Utility.RandomMinMax(-5, 5), Y + Utility.RandomMinMax(-5, 5), Z);
                minion.MoveToWorld(loc, Map);
            }

            m_NextSummonMinions = DateTime.UtcNow + TimeSpan.FromMinutes(2);
        }

        private void ActivateDesperateDefense()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Vitrail enters a desperate defense mode! *");
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x376A, 10, 16, 0x21, 0, 5020, 0);

            SetResistance(ResistanceType.Physical, 100, 120);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 100, 120);
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

    public class MinionGargoyle : BaseCreature
    {
        [Constructable]
        public MinionGargoyle()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a minion gargoyle";
            Body = 4; // Gargoyle body
            Hue = 1153; // Match the hue of Vitrail

            SetStr(150);
            SetDex(80);
            SetInt(50);

            SetHits(100);
            SetDamage(10, 15);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Tactics, 60.1, 70.0);
            SetSkill(SkillName.Wrestling, 50.1, 60.0);

            Fame = 2000;
            Karma = -2000;

            VirtualArmor = 30;
        }

        public MinionGargoyle(Serial serial)
            : base(serial)
        {
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
