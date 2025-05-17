using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Spells.Necromancy;

namespace Server.Mobiles
{
    [CorpseName("the remains of Eternal Woe")]
    public class EternalWoe : BaseCreature
    {
        private DateTime _nextWail;
        private DateTime _nextRiftPulse;
        private DateTime _nextSoulFragmentation;

        [Constructable]
        public EternalWoe() 
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "Eternal Woe";
            Body = 312; // Same as Abysmal Horror
            BaseSoundID = 0x451;
            Hue = 2301;

            SetStr(750, 900);
            SetDex(120, 140);
            SetInt(1000, 1200);

            SetHits(1200);
            SetDamage(25, 40);

            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Energy, 30);
            SetDamageType(ResistanceType.Cold, 30);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 20, 25);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 90, 100);
            SetResistance(ResistanceType.Energy, 75, 85);

            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.SpiritSpeak, 120.0);
            SetSkill(SkillName.Necromancy, 120.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Wrestling, 100.0);
            SetSkill(SkillName.Tactics, 100.0);

            Fame = 30000;
            Karma = -30000;
            VirtualArmor = 90;

            _nextWail = DateTime.UtcNow;
            _nextRiftPulse = DateTime.UtcNow;
            _nextSoulFragmentation = DateTime.UtcNow;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.UtcNow >= _nextWail)
            {
                WailOfAges();
            }

            if (DateTime.UtcNow >= _nextRiftPulse)
            {
                RiftPulse();
            }

            if (DateTime.UtcNow >= _nextSoulFragmentation)
            {
                SoulFragmentation();
            }
        }

        private void WailOfAges()
        {
            PublicOverheadMessage(MessageType.Emote, 0x47E, true, "* Eternal Woe lets out a scream that bends time itself! *");
            PlaySound(0x64D);

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet && m.Player)
                {
                    m.Freeze(TimeSpan.FromSeconds(2 + Utility.RandomMinMax(2, 4)));
                    m.SendMessage(0x22, "Your memories are fractured... you feel lost in time.");
                    Effects.SendBoltEffect(m, true);
                }
            }

            _nextWail = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void RiftPulse()
        {
            PublicOverheadMessage(MessageType.Emote, 0x47E, true, "* The ground cracks as a pulse from the Umbral Veil erupts! *");
            PlaySound(0x5A9);

            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 10, 20, 0, 0, 5023, 0);

            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && m.Alive && m is PlayerMobile)
                {
                    int damage = Utility.RandomMinMax(25, 50);
                    AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);
                    m.SendMessage("A pulse from the Rift tears through your spirit!");
                }
            }

            _nextRiftPulse = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void SoulFragmentation()
        {
            PublicOverheadMessage(MessageType.Emote, 0x47E, true, "* Eternal Woe siphons fragments of nearby souls. *");
            PlaySound(0x1FE);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && m is PlayerMobile)
                {
                    m.FixedParticles(0x375A, 10, 25, 5035, EffectLayer.Head);
                    int damage = Utility.RandomMinMax(20, 35);
                    AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);
                    Hits += damage / 2;
                    m.SendMessage("Your soul is partially devoured...");
                }
            }

            _nextSoulFragmentation = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, 5);

            if (Utility.RandomDouble() < 0.005)
            {
                PackItem(new UmbralCloakOfWoe()); // Rare unique item
            }
        }

        public override bool AutoDispel => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override bool Unprovokable => true;
        public override bool AreaPeaceImmune => true;
        public override int TreasureMapLevel => 5;

        public EternalWoe(Serial serial) : base(serial) { }

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

    public class UmbralCloakOfWoe : Cloak
    {
        [Constructable]
        public UmbralCloakOfWoe() : base()
        {
            Name = "Umbral Cloak of Woe";
            Hue = 2301;
            LootType = LootType.Blessed;
            Attributes.LowerManaCost = 10;
            Attributes.BonusInt = 5;
            Attributes.CastRecovery = 2;
            Attributes.SpellDamage = 8;
        }

        public UmbralCloakOfWoe(Serial serial) : base(serial) { }

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
