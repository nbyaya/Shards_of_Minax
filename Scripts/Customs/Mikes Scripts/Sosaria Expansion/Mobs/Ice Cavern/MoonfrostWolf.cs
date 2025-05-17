using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections;

namespace Server.Mobiles
{
    [CorpseName("a moonfrost wolf corpse")]
    public class MoonfrostWolf : BaseCreature
    {
        private DateTime m_NextFrostNova;
        private DateTime m_NextMoonPhase;
        private DateTime m_NextPhaseOut;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public MoonfrostWolf()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Moonfrost Wolf";
            Body = 250;
            Hue = 1153; // Icy lunar blue
            BaseSoundID = 0x52B;

            SetStr(700, 850);
            SetDex(180, 220);
            SetInt(200, 250);

            SetHits(900, 1100);
            SetStam(200);
            SetMana(300);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Cold, 40);

            SetResistance(ResistanceType.Physical, 55, 75);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 70, 90);
            SetResistance(ResistanceType.Poison, 60, 75);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.MagicResist, 80.0, 120.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 95.0, 110.0);
            SetSkill(SkillName.Meditation, 60.0, 80.0);
            SetSkill(SkillName.SpiritSpeak, 90.0, 100.0);

            Fame = 20000;
            Karma = -20000;

            VirtualArmor = 75;

            Tamable = false;

            m_AbilitiesInitialized = false;
        }

        public MoonfrostWolf(Serial serial) : base(serial) { }

        public override bool CanAngerOnTame => false;
        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => true;
        public override int TreasureMapLevel => 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Gems, 5);

            if (Utility.RandomDouble() < 0.005) // rare drop
                PackItem(new MoonfrostShard());
        }

        public override int GetAngerSound() => 0x52D;
        public override int GetIdleSound() => 0x52C;
        public override int GetAttackSound() => 0x52B;
        public override int GetHurtSound() => 0x52E;
        public override int GetDeathSound() => 0x52A;

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    var rand = new Random();
                    m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 30));
                    m_NextMoonPhase = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(15, 45));
                    m_NextPhaseOut = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 60));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextFrostNova)
                    CastFrostNova();

                if (DateTime.UtcNow >= m_NextMoonPhase)
                    ShiftMoonPhase();

                if (DateTime.UtcNow >= m_NextPhaseOut)
                    PhaseOut();
            }
        }

        private void CastFrostNova()
        {
            PublicOverheadMessage(MessageType.Regular, 0x47F, true, "* releases a freezing nova! *");
            PlaySound(0x10B);

            Effects.SendLocationEffect(Location, Map, 0x3709, 20, 10, Hue, 0);
            AOS.Damage(Combatant, this, Utility.RandomMinMax(30, 50), 0, 100, 0, 0, 0);

            if (Combatant is Mobile target)
            {
                target.Freeze(TimeSpan.FromSeconds(2));
                target.SendMessage(0x480, "You are frozen by lunar frost!");
            }

            m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void ShiftMoonPhase()
        {
            PublicOverheadMessage(MessageType.Regular, 0x47F, true, "* aligns with the waxing moon... *");
            PlaySound(0x1FB);

            Hits += 50;
            Mana += 40;

            if (Hits > HitsMax) Hits = HitsMax;
            if (Mana > ManaMax) Mana = ManaMax;

            m_NextMoonPhase = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void PhaseOut()
        {
            PublicOverheadMessage(MessageType.Regular, 0x47F, true, "* fades into lunar shadow... *");
            PlaySound(0x20A);

            Hidden = true;
            Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
            {
                Hidden = false;
                PublicOverheadMessage(MessageType.Regular, 0x47F, true, "* reappears from the shadow! *");
                PlaySound(0x64F);
            });

            m_NextPhaseOut = DateTime.UtcNow + TimeSpan.FromSeconds(60);
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

    public class MoonfrostShard : Item
    {
        public MoonfrostShard() : base(0x1F14)
        {
            Hue = 1153;
            Name = "Moonfrost Shard";
            LootType = LootType.Regular;
            Weight = 1.0;
        }

        public MoonfrostShard(Serial serial) : base(serial) { }

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
