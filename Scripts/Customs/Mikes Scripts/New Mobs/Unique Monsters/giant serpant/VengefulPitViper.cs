using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a vengeful pit viper corpse")]
    public class VengefulPitViper : BaseCreature
    {
        private DateTime m_NextFangedFrenzy;
        private DateTime m_NextPitSpirits;
        private DateTime m_NextSerpentsFury;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public VengefulPitViper()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Vengeful Pit Viper";
            Body = 0x15; // Giant Serpent body
            Hue = 1770; // Unique hue
			BaseSoundID = 219;

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

        public VengefulPitViper(Serial serial)
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
                    m_NextFangedFrenzy = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextPitSpirits = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextSerpentsFury = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 90));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextFangedFrenzy)
                {
                    FangedFrenzy();
                }

                if (DateTime.UtcNow >= m_NextPitSpirits)
                {
                    PitSpirits();
                }

                if (DateTime.UtcNow >= m_NextSerpentsFury)
                {
                    SerpentsFury();
                }
            }
        }

        private void FangedFrenzy()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Vengeful Pit Viper strikes with fury! *");

                for (int i = 0; i < 5; i++)
                {
                    Timer.DelayCall(TimeSpan.FromSeconds(i * 0.5), () =>
                    {
                        if (Combatant != null && !Combatant.Deleted)
                        {
                            Mobile mobileCombatant = Combatant as Mobile;
                            if (mobileCombatant != null)
                            {
                                mobileCombatant.SendMessage("You feel a sharp bite!");
                                mobileCombatant.FixedParticles(0x374A, 10, 30, 5013, EffectLayer.Head);
                                mobileCombatant.PlaySound(0x231);
                                AOS.Damage(mobileCombatant, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);

                                if (Utility.RandomDouble() < 0.5)
                                    mobileCombatant.ApplyPoison(this, Poison.Greater);
                            }
                        }
                    });
                }

                m_NextFangedFrenzy = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            }
        }

        private void PitSpirits()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Vengeful Pit Viper summons vengeful spirits! *");
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x376A, 10, 30, 0x22, 0, 5013, 0);

                for (int i = 0; i < 3; i++)
                {
                    Point3D loc = GetSpawnPosition(5);
                    if (loc != Point3D.Zero)
                    {
                        PitSpirit spirit = new PitSpirit(this);
                        spirit.MoveToWorld(loc, Map);
                    }
                }

                m_NextPitSpirits = DateTime.UtcNow + TimeSpan.FromMinutes(2);
            }
        }

        private void SerpentsFury()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Vengeful Pit Viper’s fury is unleashed! *");
                this.AddStatMod(new StatMod(StatType.Str, "SerpentsFury", 50, TimeSpan.FromSeconds(20)));
                this.AddStatMod(new StatMod(StatType.Dex, "SerpentsFury", 50, TimeSpan.FromSeconds(20)));
                this.AddStatMod(new StatMod(StatType.Int, "SerpentsFury", 25, TimeSpan.FromSeconds(20)));

                m_NextSerpentsFury = DateTime.UtcNow + TimeSpan.FromMinutes(3);
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

            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }

    public class PitSpirit : BaseCreature
    {
        private Mobile m_Master;

        public PitSpirit(Mobile master)
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            m_Master = master;

            Body = 0x13E; // Spirit body
            Hue = 1153; // Unique hue
            Name = "a pit spirit";

            SetStr(150);
            SetDex(80);
            SetInt(100);

            SetHits(100);
            SetMana(0);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 20, 30);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.MagicResist, 60.0, 80.0);
            SetSkill(SkillName.Tactics, 60.0, 80.0);
            SetSkill(SkillName.Wrestling, 60.0, 80.0);

            Fame = 1500;
            Karma = -1500;

            VirtualArmor = 20;
        }

        public PitSpirit(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

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
