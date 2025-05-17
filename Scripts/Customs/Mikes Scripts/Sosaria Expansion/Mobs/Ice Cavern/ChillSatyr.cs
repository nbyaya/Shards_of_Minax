using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Targeting;
using Server.Network;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("a chill satyr's corpse")]
    public class ChillSatyr : BaseCreature
    {
        private DateTime m_NextFrostAura;
        private DateTime m_NextFrozenPulse;
        private DateTime m_NextLullaby;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public ChillSatyr()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Chill Satyr";
            Body = 271; // Satyr body
            Hue = 1152; // Frosty blue hue
            BaseSoundID = 0x586;

            SetStr(400, 500);
            SetDex(250, 280);
            SetInt(300, 350);

            SetHits(800, 1000);

            SetDamage(18, 26);

            SetDamageType(ResistanceType.Cold, 60);
            SetDamageType(ResistanceType.Physical, 40);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 80, 95);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 40, 55);

            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 80.0, 100.0);
            SetSkill(SkillName.Musicianship, 120.0);
            SetSkill(SkillName.Peacemaking, 120.0);
            SetSkill(SkillName.Provocation, 100.0);
            SetSkill(SkillName.Discordance, 100.0);

            Fame = 16000;
            Karma = -16000;

            VirtualArmor = 50;

            m_AbilitiesInitialized = false;
        }

        public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Gems, 5);

        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && Alive)
            {
                if (!m_AbilitiesInitialized)
                {
                    m_NextFrostAura = DateTime.UtcNow + TimeSpan.FromSeconds(10);
                    m_NextFrozenPulse = DateTime.UtcNow + TimeSpan.FromSeconds(20);
                    m_NextLullaby = DateTime.UtcNow + TimeSpan.FromSeconds(30);
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextFrostAura)
                    ActivateFrostAura();

                if (DateTime.UtcNow >= m_NextFrozenPulse)
                    CastFrozenPulse();

                if (DateTime.UtcNow >= m_NextLullaby)
                    UseLullaby();
            }
        }

        private void ActivateFrostAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "*Icy mist swirls around the Chill Satyr*");
            Effects.SendLocationEffect(Location, Map, 0x374A, 16, 1, Hue, 0);
            PlaySound(0x10B); // Chilling whoosh

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    if (m is Mobile target)
                    {
                        AOS.Damage(target, this, Utility.RandomMinMax(10, 20), 0, 0, 100, 0, 0);
                        target.Freeze(TimeSpan.FromSeconds(2));
                        target.SendMessage("You feel an icy chill slow your movements!");
                    }
                }
            }

            m_NextFrostAura = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void CastFrozenPulse()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "*The Chill Satyr unleashes a wave of freezing energy!*");
            PlaySound(0x15E); // Magic burst

            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    if (m is Mobile target)
                    {
                        int manaDrain = Utility.RandomMinMax(15, 30);
                        target.Mana -= manaDrain;
                        AOS.Damage(target, this, Utility.RandomMinMax(20, 35), 0, 0, 100, 0, 0);
                        target.SendMessage("A frozen pulse saps your energy and chills your core!");
                    }
                }
            }

            m_NextFrozenPulse = DateTime.UtcNow + TimeSpan.FromSeconds(40);
        }

        private void UseLullaby()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "*The Chill Satyr hums a soothing, dangerous lullaby...*");
            PlaySound(0x54A); // Mesmerizing tune

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    if (m is Mobile target && Utility.RandomDouble() < 0.35) // 35% chance to succeed
                    {
                        target.Freeze(TimeSpan.FromSeconds(4));
                        target.SendMessage("You fall under a sleepy trance!");
                    }
                }
            }

            m_NextLullaby = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.01) // 1% drop rate
            {
                c.DropItem(new Item() { Name = "Frostsong Panpipes", Hue = 1152, LootType = LootType.Blessed });
            }
        }

        public ChillSatyr(Serial serial) : base(serial) { }

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
