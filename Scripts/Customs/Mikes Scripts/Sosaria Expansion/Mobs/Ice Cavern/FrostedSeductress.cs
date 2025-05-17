using System;
using Server;
using Server.Mobiles;
using Server.Spells;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a frosted seductress corpse")]
    public class FrostedSeductress : BaseCreature
    {
        private DateTime m_NextCharm;
        private DateTime m_NextFrostNova;
        private DateTime m_NextSoulDrain;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public FrostedSeductress()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Frosted Seductress";
            Body = 149; // Succubus body
            Hue = 1152; // Icy-blue hue
            BaseSoundID = 0x4B0;

            SetStr(600, 750);
            SetDex(200, 250);
            SetInt(1000, 1200);

            SetHits(850, 1000);

            SetDamage(22, 34);
            SetDamageType(ResistanceType.Cold, 60);
            SetDamageType(ResistanceType.Energy, 20);
            SetDamageType(ResistanceType.Physical, 20);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 80, 100);
            SetResistance(ResistanceType.Poison, 40, 55);
            SetResistance(ResistanceType.Energy, 65, 80);

            SetSkill(SkillName.Magery, 110.0, 120.0);
            SetSkill(SkillName.MagicResist, 120.0, 140.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Wrestling, 85.0, 100.0);
            SetSkill(SkillName.Meditation, 100.0, 120.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);

            Fame = 26000;
            Karma = -26000;

            VirtualArmor = 90;

            m_AbilitiesInitialized = false;
        }

        public FrostedSeductress(Serial serial)
            : base(serial)
        {
        }
        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.MedScrolls, 2);

            if (Utility.RandomDouble() < 0.002) // Rare drop
                PackItem(new IceboundVeil()); // Custom item
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    m_NextCharm = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(10, 20));
                    m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(15, 25));
                    m_NextSoulDrain = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(20, 30));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextCharm)
                    CastCharm();

                if (DateTime.UtcNow >= m_NextFrostNova)
                    FrostNova();

                if (DateTime.UtcNow >= m_NextSoulDrain)
                    SoulDrain();
            }
        }

        private void CastCharm()
        {
            if (Combatant is Mobile target)
            {
                PublicOverheadMessage(MessageType.Emote, 0x9C2, true, "*Her chilling gaze seduces your will...*");
                target.Frozen = true;
                target.SendMessage(0x5B5, "You are entranced by the Frosted Seductress!");

                Timer.DelayCall(TimeSpan.FromSeconds(4.0), () =>
                {
                    target.Frozen = false;
                    target.SendMessage("You break free from her icy enchantment!");
                });

                m_NextCharm = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(20, 30));
            }
        }

        private void FrostNova()
        {
            PublicOverheadMessage(MessageType.Emote, 0x480, true, "*She releases a pulse of freezing energy!*");
            PlaySound(0x10B); // Frost sound

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(25, 40);
                    AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);
                    m.Frozen = true;
                    m.SendMessage("You are frozen by the frost nova!");

                    Timer.DelayCall(TimeSpan.FromSeconds(3.0), () =>
                    {
                        m.Frozen = false;
                        m.SendMessage("The frost recedes from your limbs.");
                    });
                }
            }

            m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(25, 40));
        }

        private void SoulDrain()
        {
            if (Combatant is Mobile target)
            {
                int drain = Utility.RandomMinMax(15, 35);

                AOS.Damage(target, this, drain, 0, 100, 0, 0, 0);
                if (Mana < ManaMax)
                    Mana += drain;

                PublicOverheadMessage(MessageType.Emote, 0x22, true, "*The Frosted Seductress siphons your soul energy!*");
                PlaySound(0x1E9);
                target.SendMessage("Your soul is partially drained by the seductress...");

                m_NextSoulDrain = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(30, 50));
            }
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

    public class IceboundVeil : FancyShirt
    {
        [Constructable]
        public IceboundVeil() : base(0x2306) // Replace with actual veil item ID
        {
            Name = "Icebound Veil";
            Hue = 1152;
            Weight = 1.0;
            Attributes.SpellDamage = 10;
            Attributes.BonusMana = 5;
            LootType = LootType.Blessed;
        }

        public IceboundVeil(Serial serial) : base(serial) { }

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
