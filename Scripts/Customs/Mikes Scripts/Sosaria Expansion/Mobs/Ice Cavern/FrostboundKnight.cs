using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a frostbound corpse")]
    public class FrostboundKnight : BaseCreature
    {
        private DateTime m_NextFrostNova;
        private DateTime m_NextIcyGrip;
        private DateTime m_NextFrozenShield;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public FrostboundKnight()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Frostbound Knight";
            Body = 147; // Same as SkeletalKnight
            BaseSoundID = 451;
            Hue = 1150; // Icy blue hue

            SetStr(300, 350);
            SetDex(120, 150);
            SetInt(100, 150);

            SetHits(750, 1000);

            SetDamage(20, 30);
            SetDamageType(ResistanceType.Cold, 70);
            SetDamageType(ResistanceType.Physical, 30);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 90.0, 120.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);
            SetSkill(SkillName.Swords, 90.0, 120.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 65;

            PackItem(new VikingSword());
            PackItem(new MetalShield());

            m_AbilitiesInitialized = false;
        }

        public override bool BleedImmune => true;
        public override bool AutoDispel => true;
        public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;
        public override bool Unprovokable => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Gems, 4);


        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive)
                return;

            if (!m_AbilitiesInitialized)
            {
                var rand = new Random();
                m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 15));
                m_NextIcyGrip = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 25));
                m_NextFrozenShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(30, 60));
                m_AbilitiesInitialized = true;
            }

            if (DateTime.UtcNow >= m_NextFrostNova)
                FrostNova();

            if (DateTime.UtcNow >= m_NextIcyGrip)
                IcyGrip();

            if (DateTime.UtcNow >= m_NextFrozenShield)
                FrozenShield();
        }

        private void FrostNova()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "*unleashes a chilling frost nova!*");
            PlaySound(0x10B); // Ice sound
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x376A, 10, 15, Hue, 0, 5052, 0);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 100, 0, 0, 0);
                    m.Freeze(TimeSpan.FromSeconds(2));
                    m.SendMessage("A wave of freezing energy paralyzes you!");
                }
            }

            m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(20 + Utility.RandomDouble() * 20);
        }

        private void IcyGrip()
        {
            if (Combatant is Mobile target && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x480, true, "*casts Icy Grip!*");
                PlaySound(0x1F5); // Cold casting sound
                target.Freeze(TimeSpan.FromSeconds(3));
                target.SendMessage("Your limbs freeze as the Frostbound Knight grips your soul!");

                AOS.Damage(target, this, Utility.RandomMinMax(20, 35), 0, 100, 0, 0, 0);

                m_NextIcyGrip = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            }
        }

        private void FrozenShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "*summons a frozen barrier!*");
            PlaySound(0x208); // Shielding sound
            VirtualArmor += 50;

            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                VirtualArmor -= 50;
                PublicOverheadMessage(MessageType.Regular, 0x480, true, "*the frozen barrier melts away*");
            });

            m_NextFrozenShield = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        public FrostboundKnight(Serial serial)
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
            m_AbilitiesInitialized = false;
        }
    }
}
