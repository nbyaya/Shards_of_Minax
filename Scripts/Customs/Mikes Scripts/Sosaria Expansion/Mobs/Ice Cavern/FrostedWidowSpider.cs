using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a frosted widow spider corpse")]
    public class FrostedWidowSpider : BaseCreature
    {
        private DateTime m_NextWebBlast;
        private DateTime m_NextVenomFreeze;
        private DateTime m_NextPhaseShift;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public FrostedWidowSpider()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Frosted Widow Spider";
            Body = 736; // Same spider body
            Hue = 1152; // Unique frosted icy-blue hue
            BaseSoundID = 1605; // Use same sound profile as WolfSpider

            SetStr(450, 520);
            SetDex(210, 245);
            SetInt(410, 470);

            SetHits(650, 800);
            SetMana(500, 650);
            SetStam(210, 245);

            SetDamage(18, 26);

            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Cold, 45);
            SetDamageType(ResistanceType.Poison, 15);

            SetResistance(ResistanceType.Physical, 45, 60);
            SetResistance(ResistanceType.Fire, 15, 25);
            SetResistance(ResistanceType.Cold, 70, 90);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.MagicResist, 100.0, 125.0);
            SetSkill(SkillName.Poisoning, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Wrestling, 85.0, 95.0);
            SetSkill(SkillName.Magery, 95.0, 105.0);
            SetSkill(SkillName.EvalInt, 90.0, 110.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 75;
            Tamable = false;

            m_AbilitiesInitialized = false;
        }

        public FrostedWidowSpider(Serial serial)
            : base(serial)
        {
        }

        public override void GenerateLoot()
        {
            PackItem(new SpidersSilk(10));
            AddLoot(LootPack.FilthyRich, 1);
            AddLoot(LootPack.Gems, 2);

            if (Utility.RandomDouble() < 0.005) // 1 in 200 chance
                PackItem(new IceVenomSac());
        }

        public override Poison PoisonImmune => Poison.Deadly;
        public override Poison HitPoison => Poison.Greater;

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null)
                return;

            if (!m_AbilitiesInitialized)
            {
                m_NextWebBlast = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 15));
                m_NextVenomFreeze = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20));
                m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30));
                m_AbilitiesInitialized = true;
            }

            if (DateTime.UtcNow >= m_NextWebBlast)
                WebBlast();

            if (DateTime.UtcNow >= m_NextVenomFreeze)
                VenomFreeze();

            if (DateTime.UtcNow >= m_NextPhaseShift)
                PhaseShift();
        }

        private void WebBlast()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "* The Frosted Widow hurls a burst of frost-webs! *");

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    m.Frozen = true;
                    Timer.DelayCall(TimeSpan.FromSeconds(3.5), () => m.Frozen = false);
                    m.SendMessage(1152, "You are entangled in freezing webs!");
                    m.PlaySound(0x64F); // Web/freeze sound
                }
            }

            m_NextWebBlast = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 35));
        }

        private void VenomFreeze()
        {
            if (Combatant is Mobile target)
            {
                PublicOverheadMessage(MessageType.Regular, 0x480, true, "* The spider injects venom that crystallizes your blood! *");

                target.Freeze(TimeSpan.FromSeconds(2));
                AOS.Damage(target, this, Utility.RandomMinMax(20, 35), 0, 0, 100, 0, 0);
                target.SendMessage(1152, "Your limbs seize with icy venom!");
                target.PlaySound(0x1FB);
            }

            m_NextVenomFreeze = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
        }

        private void PhaseShift()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "* The Frosted Widow blinks into a veil of frost... *");

            Effects.SendLocationEffect(Location, Map, 0x3728, 10);
            this.Hidden = true;

            Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
            {
                this.Hidden = false;
                Effects.SendLocationEffect(Location, Map, 0x3728, 10);
                Say("*It re-emerges with frigid silence...*");
            });

            m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 40));
        }

        public override int GetIdleSound() => 1605;
        public override int GetAngerSound() => 1602;
        public override int GetHurtSound() => 1604;
        public override int GetDeathSound() => 1603;

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (!Controlled && Utility.RandomDouble() < 0.02)
                c.DropItem(new LuckyCoin()); // Shared rare drop
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

    public class IceVenomSac : Item
    {
        [Constructable]
        public IceVenomSac() : base(0xF0A) // A unique appearance, customize if needed
        {
            Name = "an Ice Venom Sac";
            Hue = 1152;
            Weight = 1.0;
        }

        public IceVenomSac(Serial serial) : base(serial)
        {
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
