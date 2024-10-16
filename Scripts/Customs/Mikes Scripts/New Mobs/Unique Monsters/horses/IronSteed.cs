using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an iron steed corpse")]
    public class IronSteed : BaseMount
    {
        private DateTime m_NextIronArmor;
        private DateTime m_NextMetallicRoar;
        private bool m_ReflectiveShieldActive;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public IronSteed()
            : base("Iron Steed", 0xE2, 0x3EA0, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Body = 0xE2;
            ItemID = 0x3EA0;
            Hue = 2089; // Metallic silver hue
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

            m_ReflectiveShieldActive = true;
            m_AbilitiesInitialized = false; // Initialize flag
        }

        public IronSteed(Serial serial)
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
                    m_NextIronArmor = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextMetallicRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextIronArmor)
                {
                    IronArmor();
                }

                if (DateTime.UtcNow >= m_NextMetallicRoar)
                {
                    MetallicRoar();
                }
            }
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (m_ReflectiveShieldActive && from != null && from != this && 0.25 > Utility.RandomDouble())
            {
                int reflectedDamage = amount / 4;
                from.Damage(reflectedDamage, this);
                this.Say("*The attack is partially reflected!*");
            }
        }

        private void IronArmor()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Iron Steed's armor hardens! *");
            PlaySound(0x1E3);
            FixedEffect(0x376A, 10, 16);

            ResistanceMod[] mods = new ResistanceMod[]
            {
                new ResistanceMod(ResistanceType.Physical, 50),
                new ResistanceMod(ResistanceType.Fire, 50),
                new ResistanceMod(ResistanceType.Cold, 50),
                new ResistanceMod(ResistanceType.Poison, 50),
                new ResistanceMod(ResistanceType.Energy, 50)
            };

            for (int i = 0; i < mods.Length; ++i)
                AddResistanceMod(mods[i]);

            Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(delegate
            {
                for (int i = 0; i < mods.Length; ++i)
                    RemoveResistanceMod(mods[i]);
            }));

            m_NextIronArmor = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Reset the cooldown
        }

        private void MetallicRoar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Iron Steed lets out a deafening roar! *");
            PlaySound(0x16A);
            FixedEffect(0x37C4, 10, 36);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    m.Freeze(TimeSpan.FromSeconds(3));
                    m.FixedParticles(0x374A, 10, 15, 5013, 0x455, 0, EffectLayer.Waist);
                    m.PlaySound(0x204);

                    if (m.Player)
                        m.SendLocalizedMessage(1072061); // You have been stunned by a concussion blow!
                }
            }

            m_NextMetallicRoar = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Reset the cooldown
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            writer.Write(m_ReflectiveShieldActive);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_ReflectiveShieldActive = reader.ReadBool();

            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }
}
