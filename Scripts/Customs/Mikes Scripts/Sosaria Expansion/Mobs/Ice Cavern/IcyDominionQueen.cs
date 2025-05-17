using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("the icy dominion queen's corpse")]
    public class IcyDominionQueen : BaseCreature
    {
        private DateTime m_NextFrostNova;
        private DateTime m_NextClone;
        private DateTime m_NextManaDrain;
        private bool m_Initialized;

        [Constructable]
        public IcyDominionQueen() 
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.3, 0.5)
        {
            Name = "the Icy Dominion Queen";
            Body = 807;
            BaseSoundID = 959;
            Hue = 0x480; // A cold blue-white shimmer



            SetStr(500, 600);
            SetDex(150, 180);
            SetInt(300, 400);

            SetHits(2000, 2500);
            SetMana(1500, 1800);

            SetDamage(15, 20);
            SetDamageType(ResistanceType.Cold, 100);

            SetResistance(ResistanceType.Physical, 45, 60);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.EvalInt, 90.0, 110.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.Wrestling, 75.0, 90.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 80;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!m_Initialized)
            {
                m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
                m_NextClone = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30));
                m_NextManaDrain = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 40));
                m_Initialized = true;
            }

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextFrostNova)
                    CastFrostNova();

                if (DateTime.UtcNow >= m_NextClone)
                    SummonIceClone();

                if (DateTime.UtcNow >= m_NextManaDrain)
                    ManaFreezeBlast();
            }
        }

        private void CastFrostNova()
        {
            PublicOverheadMessage(MessageType.Emote, 0x480, true, "*The air crystallizes around the Icy Queen!*");
            PlaySound(0x64C); // Cold blast sound

            Effects.SendLocationEffect(Location, Map, 0x3779, 20, 10); // Ice burst

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.Freeze(TimeSpan.FromSeconds(2));
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 40), 0, 0, 100, 0, 0);
                    m.SendMessage("You are caught in the Frost Nova!");
                }
            }

            m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void SummonIceClone()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "*She spawns a glacial reflection!*");
            PlaySound(0x658);

            IceClone clone = new IceClone(this);
            clone.MoveToWorld(Location, Map);

            m_NextClone = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void ManaFreezeBlast()
        {
            if (Combatant is Mobile target)
            {
                PublicOverheadMessage(MessageType.Regular, 0x480, true, "*She unleashes a blast of freezing will!*");
                PlaySound(0x64C);

                int drained = Utility.RandomMinMax(20, 40);
                target.Mana -= drained;
                target.Stam -= drained;

                target.SendMessage(0x480, "Your energy is leeched by the Icy Dominion Queen!");
            }

            m_NextManaDrain = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.01)
                c.DropItem(new IceCrown()); // Unique rare loot
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Rich, 2);
            AddLoot(LootPack.Gems, 5);
        }

        public override int GetAngerSound() => 0x259;
        public override int GetIdleSound() => 0x259;
        public override int GetAttackSound() => 0x195;
        public override int GetHurtSound() => 0x250;
        public override int GetDeathSound() => 0x25B;

        public IcyDominionQueen(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Initialized = false;
        }
    }

    public class IceClone : BaseCreature
    {
        private Timer m_SelfDestruct;

        public IceClone(IcyDominionQueen queen)
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a glacial reflection";
            Body = 807;
            Hue = 0x47F;
            BaseSoundID = 959;
            Team = queen.Team;

            SetStr(100);
            SetDex(100);
            SetInt(10);

            SetHits(150);
            SetDamage(5, 10);
            SetDamageType(ResistanceType.Cold, 100);

            VirtualArmor = 20;

            m_SelfDestruct = Timer.DelayCall(TimeSpan.FromSeconds(15), Explode);
        }

        public IceClone(Serial serial)
            : base(serial)
        {
        }

        private void Explode()
        {
            if (Deleted || !Alive) return;

            PlaySound(0x10B);
            Effects.SendLocationEffect(Location, Map, 0x36BD, 20, 10);
            foreach (Mobile m in GetMobilesInRange(2))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 25), 0, 0, 100, 0, 0);
                    m.SendMessage("The ice clone explodes in a freezing burst!");
                }
            }

            Delete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }

    public class IceCrown : Item
    {
        public IceCrown() : base(0x2B70)
        {
            Name = "Glacial Crown of Dominion";
            Hue = 0x480;
            LootType = LootType.Blessed;
        }

        public IceCrown(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
