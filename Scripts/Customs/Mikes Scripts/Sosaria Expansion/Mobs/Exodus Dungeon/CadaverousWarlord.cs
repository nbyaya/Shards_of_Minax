using System;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("a cadaverous husk")]
    public class CadaverousWarlord : BaseCreature
    {
        private DateTime m_NextSoulRip;
        private DateTime m_NextRaiseFallen;
        private DateTime m_NextWarCry;
        private bool m_Initialized;

        [Constructable]
        public CadaverousWarlord()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Cadaverous Warlord";
            Body = 7;
            BaseSoundID = 0x45A;
            Hue = 2610;

            SetStr(850, 1050);
            SetDex(90, 120);
            SetInt(500, 650);

            SetHits(1400, 1800);

            SetDamage(25, 40);
            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Cold, 30);

            SetResistance(ResistanceType.Physical, 60, 80);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Poison, 70, 85);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Swords, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.SpiritSpeak, 90.0, 100.0);
            SetSkill(SkillName.Necromancy, 100.0, 110.0);

            Fame = 28000;
            Karma = -28000;

            VirtualArmor = 75;

            m_Initialized = false;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_Initialized)
                {
                    m_NextSoulRip = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20));
                    m_NextRaiseFallen = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
                    m_NextWarCry = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
                    m_Initialized = true;
                }

                if (DateTime.UtcNow >= m_NextSoulRip)
                    SoulRip();

                if (DateTime.UtcNow >= m_NextRaiseFallen)
                    RaiseFallen();

                if (DateTime.UtcNow >= m_NextWarCry)
                    WarCry();
            }
        }

        private void SoulRip()
        {
            if (Combatant is Mobile target)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Warlord tears at your soul!*");
                PlaySound(0x1E9);

                int damage = Utility.RandomMinMax(35, 50);
                AOS.Damage(target, this, damage, 0, 0, 0, 0, 100); // Pure energy
                target.Mana -= Utility.RandomMinMax(10, 20);

                target.SendMessage(38, "Your soul burns and your energy is drained!");

                m_NextSoulRip = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 35));
            }
        }

        private void RaiseFallen()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Warlord raises the dead to his cause!*");
            PlaySound(0x1F2);
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x37CC, 10, 20, 2023);

            int count = 0;
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m.Corpse != null && m.Corpse.Deleted == false && count < 3)
                {
                    var undead = new WarlordMinion();
                    undead.MoveToWorld(m.Corpse.Location, Map);
                    undead.Combatant = this.Combatant;
                    m.Corpse.Delete();
                    count++;
                }
            }

            m_NextRaiseFallen = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void WarCry()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Cadaverous Warlord lets out a terrifying war cry!*");
            PlaySound(0x2A3);

            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m is Mobile target && m != this && target.Alive && target.Player)
                {
                    AOS.Damage(target, this, Utility.RandomMinMax(15, 25), 100, 0, 0, 0, 0);
                    target.Stam -= 15;
                    target.SendMessage(38, "You are staggered by the Warlord's presence!");
                }
            }

            m_NextWarCry = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, Utility.Random(4, 6));

            if (Utility.RandomDouble() < 0.05)
                PackItem(new WarlordHelm()); // Custom rare drop
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            c.DropItem(new BonePile()); // Visual flavor
        }

        public override bool CanRummageCorpses => true;
        public override bool BleedImmune => true;
        public override Poison HitPoison => Poison.Lethal;
        public override Poison PoisonImmune => Poison.Lethal;

        public CadaverousWarlord(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Initialized = false;
        }
    }

    public class WarlordMinion : BaseCreature
    {
        [Constructable]
        public WarlordMinion() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "undead soldier";
            Body = 50;
            Hue = 2609;
            BaseSoundID = 471;

            SetStr(80);
            SetDex(60);
            SetInt(20);

            SetHits(120);

            SetDamage(5, 10);

            SetResistance(ResistanceType.Physical, 30);
            SetResistance(ResistanceType.Cold, 50);
            SetResistance(ResistanceType.Poison, 100);

            VirtualArmor = 25;

            Fame = 1000;
            Karma = -1000;
        }

        public WarlordMinion(Serial serial) : base(serial) { }

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

    public class WarlordHelm : BaseHat
    {
        public override int LabelNumber => 1154372; // "Helm of the Cadaverous Warlord"
        public override int BasePhysicalResistance => 5;

        [Constructable]
        public WarlordHelm() : base(0x1412)
        {
            Weight = 3.0;
            Hue = 2610;
            Name = "Helm of the Cadaverous Warlord";
            Attributes.BonusStr = 5;
            Attributes.WeaponDamage = 10;
            LootType = LootType.Blessed;
        }

        public WarlordHelm(Serial serial) : base(serial) { }

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
