using System;
using Server;
using Server.Mobiles;
using Server.Targeting;
using Server.Spells;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("a shiverlash corpse")]
    public class Shiverlash : BaseCreature
    {
        private DateTime m_NextCryoLash;
        private DateTime m_NextMindShriek;
        private DateTime m_NextFrostNova;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public Shiverlash()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Shiverlash";
            Body = 1407; // Lasher body
            BaseSoundID = 0xA8;
            Hue = 1153; // Pale frostblue

            SetStr(600, 800);
            SetDex(150, 200);
            SetInt(500, 650);

            SetHits(1500, 2000);
            SetDamage(25, 35);

            SetDamageType(ResistanceType.Cold, 70);
            SetDamageType(ResistanceType.Energy, 30);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Magery, 110.0, 120.0);
            SetSkill(SkillName.EvalInt, 110.0, 120.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 90.0, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 70;

            m_AbilitiesInitialized = false;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    m_NextCryoLash = DateTime.UtcNow + TimeSpan.FromSeconds(10);
                    m_NextMindShriek = DateTime.UtcNow + TimeSpan.FromSeconds(15);
                    m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(25);
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextCryoLash)
                    CryoLash();

                if (DateTime.UtcNow >= m_NextMindShriek)
                    MindShriek();

                if (DateTime.UtcNow >= m_NextFrostNova)
                    FrostNova();
            }
        }

        private void CryoLash()
        {
            PublicOverheadMessage(MessageType.Emote, 0x480, false, "* Shiverlash extends an icy lash toward its foe! *");
            PlaySound(0x10B); // Ice sound

            if (Combatant is Mobile target && target.Alive)
            {
                AOS.Damage(target, this, Utility.RandomMinMax(25, 35), 0, 0, 100, 0, 0);
                target.Freeze(TimeSpan.FromSeconds(2.5));
                target.SendMessage(1153, "Your limbs are seized by a freezing whip!");
            }

            m_NextCryoLash = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        private void MindShriek()
        {
            PublicOverheadMessage(MessageType.Emote, 0x22, false, "* A psychic shriek echoes through your mind... *");
            PlaySound(0x229); // Wail

            if (Combatant is Mobile target && target.Alive)
            {
                target.Mana -= Utility.RandomMinMax(15, 30);
                target.Stam -= Utility.RandomMinMax(10, 20);
                AOS.Damage(target, this, 15, 0, 0, 0, 0, 100);
                target.SendMessage(1153, "You reel from a mental assault!");
            }

            m_NextMindShriek = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void FrostNova()
        {
            PublicOverheadMessage(MessageType.Emote, 0x480, false, "* Shiverlash releases a burst of freezing energy! *");
            Effects.PlaySound(Location, Map, 0x64C); // Frostburst sound
            Effects.SendLocationEffect(Location, Map, 0x375A, 20, 10, Hue, 0);

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Alive && !m.Blessed && !m.IsDeadBondedPet)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 0, 100, 0, 0);
                    m.Freeze(TimeSpan.FromSeconds(1.5));
                    m.SendMessage(1153, "You are struck by a wave of glacial force!");
                }
            }

            m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(35);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.Gems, 5);
            AddLoot(LootPack.MedScrolls, 2);
        }

        public override int TreasureMapLevel => 5;
        public override bool AutoDispel => true;
        public override bool BleedImmune => true;

        public Shiverlash(Serial serial) : base(serial) { }

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
}
