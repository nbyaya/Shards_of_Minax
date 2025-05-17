using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a glacial abomination corpse")]
    public class GlacialAbomination : BaseCreature
    {
        private DateTime m_NextFrostNova;
        private DateTime m_NextShatterSoul;
        private DateTime m_NextTimeLock;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public GlacialAbomination()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Glacial Abomination";
            Body = 312; // Same body as Abyssal Abomination
            Hue = 1150; // Icy blue glow
            BaseSoundID = 0x451;

            SetStr(800, 950);
            SetDex(120, 150);
            SetInt(700, 850);

            SetHits(1200, 1500);

            SetDamage(25, 35);
            SetDamageType(ResistanceType.Cold, 75);
            SetDamageType(ResistanceType.Physical, 25);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 90, 100);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 90.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 80;

            m_AbilitiesInitialized = false;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 30));
                    m_NextShatterSoul = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(15, 40));
                    m_NextTimeLock = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(30, 60));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextFrostNova)
                    FrostNova();

                if (DateTime.UtcNow >= m_NextShatterSoul)
                    ShatterSoul();

                if (DateTime.UtcNow >= m_NextTimeLock)
                    TimeLock();
            }
        }

        private void FrostNova()
        {
            PublicOverheadMessage(MessageType.Emote, 0x480, true, "*emits a chilling burst of frost*");
            PlaySound(0x64C); // Frost sound
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, TimeSpan.FromSeconds(1)), 0x37C4, 10, 20, 1153, 2, 9502, 0);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(25, 45), 0, 0, 100, 0, 0);
                    m.Freeze(TimeSpan.FromSeconds(2));

                    if (m is Mobile target)
                        target.SendMessage("Your limbs stiffen as the cold rushes over you!");
                }
            }

            m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void ShatterSoul()
        {
            if (Combatant is Mobile target)
            {
                PublicOverheadMessage(MessageType.Regular, 0x480, true, "*focuses icy energy on its foe*");
                PlaySound(0x56A); // Chill shatter sound
                Effects.SendBoltEffect(target);

                int damage = Utility.RandomMinMax(50, 70);
                AOS.Damage(target, this, damage, 0, 0, 100, 0, 0);
                target.Mana -= Utility.RandomMinMax(10, 20);
                target.Stam -= Utility.RandomMinMax(10, 20);

                target.SendMessage("You feel part of your soul freeze and crack...");

                m_NextShatterSoul = DateTime.UtcNow + TimeSpan.FromSeconds(45);
            }
        }

        private void TimeLock()
        {
            PublicOverheadMessage(MessageType.Emote, 0x480, true, "*manipulates time with an icy pulse*");
            PlaySound(0x28E); // Time freeze sound

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.Freeze(TimeSpan.FromSeconds(3));
                    m.SendMessage("Time itself slows as your body refuses to move...");
                }
            }

            m_NextTimeLock = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 3);
            AddLoot(LootPack.Gems, 5);

            if (Utility.RandomDouble() < 0.005) // 1 in 200 chance
                PackItem(new FrostboundCore()); // Unique rare item
        }

        public override bool IgnoreYoungProtection => true;
        public override bool Unprovokable => true;
        public override bool AreaPeaceImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int TreasureMapLevel => 5;

        public GlacialAbomination(Serial serial) : base(serial) { }

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

    public class FrostboundCore : Item
    {
        [Constructable]
        public FrostboundCore() : base(0x2B70) // Example item ID
        {
            Name = "a frostbound core";
            Hue = 1153;
            Weight = 1.0;
        }

        public FrostboundCore(Serial serial) : base(serial) { }

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
