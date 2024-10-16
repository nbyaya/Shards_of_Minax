using System;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("corpse of a frostbound champion")]
    public class FrostboundChampion : BaseCreature
    {
        private DateTime m_NextIceShardBarrage;
        private DateTime m_NextFrostShield;
        private DateTime m_FrostShieldEnd;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public FrostboundChampion()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a frostbound champion";
            Body = 57; // BoneKnight body
            BaseSoundID = 451;
            Hue = 2371; // Icy blue hue

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

            PackItem(new Scimitar());
            PackItem(new WoodenShield());

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public FrostboundChampion(Serial serial)
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

        public override bool BleedImmune { get { return true; } }
        public override bool BardImmune { get { return !Core.AOS; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextIceShardBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextFrostShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_FrostShieldEnd = DateTime.MinValue; // Ensure shield end is properly reset
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextIceShardBarrage)
                {
                    IceShardBarrage();
                }

                if (DateTime.UtcNow >= m_NextFrostShield && DateTime.UtcNow >= m_FrostShieldEnd)
                {
                    FrostShield();
                }

                if (DateTime.UtcNow >= m_FrostShieldEnd && m_FrostShieldEnd != DateTime.MinValue)
                {
                    DeactivateFrostShield();
                }
            }
        }

        private void IceShardBarrage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Ice Shard Barrage *");
            PlaySound(0x15E);

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);

                    m.FixedParticles(0x374A, 10, 30, 5013, 0x480, 0, EffectLayer.Waist);
                    m.PlaySound(0x1E5);

                    // Slow effect
                    m.SendLocalizedMessage(1072221); // An icy wind surrounds you, slowing your movement.
                    m.Freeze(TimeSpan.FromSeconds(5));
                }
            }

            m_NextIceShardBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Update cooldown
        }

        private void FrostShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Frost Shield *");
            PlaySound(0x1ED);
            FixedParticles(0x376A, 9, 32, 5030, 0x480, 0, EffectLayer.Waist);

            ResistanceMod mod = new ResistanceMod(ResistanceType.Physical, 15);
            AddResistanceMod(mod);

            m_FrostShieldEnd = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            m_NextFrostShield = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Update cooldown
        }

        private void DeactivateFrostShield()
        {
            RemoveResistanceMod(new ResistanceMod(ResistanceType.Physical, 15));
            m_FrostShieldEnd = DateTime.MinValue;
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (m_FrostShieldEnd > DateTime.UtcNow && from != null && from != this && 0.25 > Utility.RandomDouble())
            {
                from.SendLocalizedMessage(1008111, false, Name); // : The cold radiating from your victim's shield chills you.
                from.Freeze(TimeSpan.FromSeconds(2));
                from.FixedParticles(0x374A, 10, 30, 5013, 0x480, 0, EffectLayer.Waist);
                from.PlaySound(0x1E5);
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
