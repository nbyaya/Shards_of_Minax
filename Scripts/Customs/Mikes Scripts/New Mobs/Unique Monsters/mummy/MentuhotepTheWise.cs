using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a Mentuhotep the Wise corpse")]
    public class MentuhotepTheWise : BaseCreature
    {
        private DateTime m_NextWisdomOfAges;
        private DateTime m_NextTemporalDisplacement;
        private DateTime m_NextCurseOfPharaohs;
        private DateTime m_NextAncientGuardians;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public MentuhotepTheWise()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Mentuhotep the Wise";
            Body = 154; // Mummy body
            Hue = 2163; // Unique hue
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

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public MentuhotepTheWise(Serial serial)
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
                    m_NextWisdomOfAges = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextTemporalDisplacement = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextCurseOfPharaohs = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextAncientGuardians = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 70));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextWisdomOfAges)
                {
                    WisdomOfAges();
                }

                if (DateTime.UtcNow >= m_NextTemporalDisplacement)
                {
                    TemporalDisplacement();
                }

                if (DateTime.UtcNow >= m_NextCurseOfPharaohs)
                {
                    CurseOfPharaohs();
                }

                if (DateTime.UtcNow >= m_NextAncientGuardians)
                {
                    SummonAncientGuardians();
                }
            }
        }

        private void WisdomOfAges()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Mentuhotep the Wise invokes the Wisdom of Ages! *");
            PlaySound(0x20E); // Magical effect sound

            // Increase magical resistance and spellcasting abilities
            this.Fame += 2500; // Temporary increase in fame to represent the effect
            this.VirtualArmor += 30; // Temporary increase in virtual armor

            FixedParticles(0x3709, 9, 32, 5000, EffectLayer.Waist); // Glowing script effect

            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                this.Fame -= 2500; // Revert the increase after effect duration
                this.VirtualArmor -= 30; // Revert the virtual armor increase
            });

            m_NextWisdomOfAges = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for WisdomOfAges
        }

        private void TemporalDisplacement()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Mentuhotep the Wise performs Temporal Displacement! *");
            PlaySound(0x20E); // Magical effect sound

            // Teleport to a random location
            Point3D newLocation = new Point3D(Utility.RandomMinMax(Location.X - 15, Location.X + 15), Utility.RandomMinMax(Location.Y - 15, Location.Y + 15), Location.Z);
            if (Map.CanFit(newLocation, 16, false, false))
            {
                this.Location = newLocation;
                Effects.SendLocationEffect(newLocation, Map, 0x373A, 10, 0); // Shimmering effect

                // Optional: Add a delay for the effect
                Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                {
                    Effects.SendLocationEffect(newLocation, Map, 0x373A, 10, 0); // Repeating shimmer effect
                });
            }

            m_NextTemporalDisplacement = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for TemporalDisplacement
        }

        private void CurseOfPharaohs()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Mentuhotep the Wise casts the Curse of Pharaohs! *");
            PlaySound(0x20E); // Magical effect sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 40), 0, 0, 100, 0, 0); // High magic damage
                    m.SendMessage("You feel a powerful curse weaken you!");

                    // Apply a debuff effect
                    m.Damage(10, this);
                    m.SendMessage("The curse of the pharaohs has sapped your strength!");
                }
            }

            m_NextCurseOfPharaohs = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for CurseOfPharaohs
        }

        private void SummonAncientGuardians()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Mentuhotep the Wise summons Ancient Guardians! *");
            PlaySound(0x20E); // Magical effect sound

            for (int i = 0; i < 2; i++)
            {
                BaseCreature guardian = new GuardianOfTheTomb();
                guardian.MoveToWorld(Location, Map);
                guardian.Combatant = this;
            }

            m_NextAncientGuardians = DateTime.UtcNow + TimeSpan.FromSeconds(90); // Cooldown for SummonAncientGuardians
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

    public class GuardianOfTheTomb : BaseCreature
    {
        [Constructable]
        public GuardianOfTheTomb()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Guardian of the Tomb";
            Body = 154; // Mummy body
            Hue = 1176; // Slightly different hue to distinguish

            SetStr(300);
            SetDex(100);
            SetInt(50);

            SetHits(150);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            SetResistance(ResistanceType.Physical, 50);
            SetResistance(ResistanceType.Fire, 20);
            SetResistance(ResistanceType.Cold, 50);
            SetResistance(ResistanceType.Poison, 20);
            SetResistance(ResistanceType.Energy, 30);

            SetSkill(SkillName.MagicResist, 60.0, 80.0);
            SetSkill(SkillName.Tactics, 60.0, 80.0);
            SetSkill(SkillName.Wrestling, 60.0, 80.0);

            Fame = 2000;
            Karma = -2000;

            VirtualArmor = 40;
        }

        public GuardianOfTheTomb(Serial serial)
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
