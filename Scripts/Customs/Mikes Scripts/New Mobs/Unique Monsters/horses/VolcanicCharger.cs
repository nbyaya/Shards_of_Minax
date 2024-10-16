using System;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a volcanic charger corpse")]
    public class VolcanicCharger : BaseMount
    {
        private DateTime m_NextLavaEruption;
        private DateTime m_NextMoltenArmor;
        private DateTime m_NextSeismicImpact;
        private bool m_MoltenArmorActive;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public VolcanicCharger()
            : base("Volcanic Charger", 0xE2, 0x3EA0, AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Body = 0xE2;
            ItemID = 0x3EA0;
            Hue = 2085; // Fiery red hue
            BaseSoundID = 0xA8;

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

        public VolcanicCharger(Serial serial)
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

        public override int Meat { get { return 3; } }
        public override int Hides { get { return 10; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextLavaEruption = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 30));
                    m_NextMoltenArmor = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 60));
                    m_NextSeismicImpact = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(15, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextLavaEruption)
                {
                    LavaEruption();
                }

                if (DateTime.UtcNow >= m_NextMoltenArmor && !m_MoltenArmorActive)
                {
                    MoltenArmor();
                }

                if (DateTime.UtcNow >= m_NextSeismicImpact)
                {
                    SeismicImpact();
                }
            }
        }

        private void LavaEruption()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Lava erupts from the Volcanic Charger! *");
            PlaySound(0x2F3);
            FixedEffect(0x36B0, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    int damage = Utility.RandomMinMax(30, 40);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);

                    m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                    m.PlaySound(0x208);

                    // Add damage over time effect
                    Timer.DelayCall(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5), 3, () =>
                    {
                        if (m.Alive)
                        {
                            int dotDamage = Utility.RandomMinMax(10, 15);
                            AOS.Damage(m, this, dotDamage, 0, 100, 0, 0, 0);
                            m.FixedParticles(0x36B0, 10, 15, 5052, EffectLayer.LeftFoot);
                        }
                    });
                }
            }

            m_NextLavaEruption = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void MoltenArmor()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Molten armor surrounds the Volcanic Charger! *");
            PlaySound(0x104);

            ResistanceMod[] mods = new ResistanceMod[]
            {
                new ResistanceMod(ResistanceType.Physical, 15),
                new ResistanceMod(ResistanceType.Fire, 15),
                new ResistanceMod(ResistanceType.Cold, 15),
                new ResistanceMod(ResistanceType.Poison, 15),
                new ResistanceMod(ResistanceType.Energy, 15)
            };

            foreach (ResistanceMod mod in mods)
                AddResistanceMod(mod);

            m_MoltenArmorActive = true;

            Timer.DelayCall(TimeSpan.FromSeconds(20), () =>
            {
                foreach (ResistanceMod mod in mods)
                    RemoveResistanceMod(mod);

                m_MoltenArmorActive = false;
            });

            m_NextMoltenArmor = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void SeismicImpact()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The ground trembles beneath the Volcanic Charger! *");
            PlaySound(0x2F3);
            FixedEffect(0x36B0, 10, 16);

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);

                    m.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                    m.PlaySound(0x491);

                    // Stun effect
                    m.Freeze(TimeSpan.FromSeconds(2));
                    m.SendLocalizedMessage(1080368); // You have been stunned by the tremors in the ground!
                }
            }

            m_NextSeismicImpact = DateTime.UtcNow + TimeSpan.FromSeconds(25);
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
            m_NextLavaEruption = DateTime.UtcNow;
            m_NextMoltenArmor = DateTime.UtcNow;
            m_NextSeismicImpact = DateTime.UtcNow;
            m_MoltenArmorActive = false;
        }
    }
}
