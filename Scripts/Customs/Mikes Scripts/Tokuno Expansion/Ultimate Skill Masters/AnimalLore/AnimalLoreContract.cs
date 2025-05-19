using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Items;
using Server.Targeting;
using System.Linq;

namespace Server.Items
{
    [Flipable(0x14EF, 0x14F0)]
    public class AnimalLoreContract : Item
    {
        private int m_CreatureType;
        private int m_GoldReward;
        private int m_AmountToTame;
        private int m_AmountTamed;
        private SkillName m_PowerScrollSkill;
        private double m_PowerScrollValue;

        [CommandProperty(AccessLevel.GameMaster)]
        public int CreatureType
        {
            get { return m_CreatureType; }
            set { m_CreatureType = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int GoldReward
        {
            get { return m_GoldReward; }
            set { m_GoldReward = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AmountToTame
        {
            get { return m_AmountToTame; }
            set { m_AmountToTame = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AmountTamed
        {
            get { return m_AmountTamed; }
            set { m_AmountTamed = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public SkillName PowerScrollSkill
        {
            get { return m_PowerScrollSkill; }
            set { m_PowerScrollSkill = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public double PowerScrollValue
        {
            get { return m_PowerScrollValue; }
            set { m_PowerScrollValue = value; }
        }

        [Constructable]
        public AnimalLoreContract(PlayerMobile player) : base(0x14EF)
        {
            Weight = 1.0;
            Movable = true;
            LootType = LootType.Blessed;

            // Filter creatures based on player's taming skill
            var candidates = AnimalLoreContractType.Get
                .Where(ct =>
                {
                    try
                    {
                        var bc = Activator.CreateInstance(ct.Type) as BaseCreature;
                        return bc != null
                            && bc.Tamable
                            && player.Skills[SkillName.AnimalTaming].Value >= bc.MinTameSkill;
                    }
                    catch
                    {
                        return false;
                    }
                })
                .ToList();

            // 2) If none available, give a no-quest placeholder
            if (candidates.Count == 0)
            {
                Name = "Animal Lore Contract: No tamable creatures available";
                return;
            }

            // 3) Pick one at random from the valid subset
            var selected = candidates[Utility.Random(candidates.Count)];

            // 4) Store its GLOBAL index, not the subset index
            m_CreatureType = Array.IndexOf(AnimalLoreContractType.Get, selected);

            // 5) Set up amounts, reward and name
            m_AmountToTame = Utility.RandomMinMax(5, 10);
            m_GoldReward   = m_AmountToTame * 500;
            m_AmountTamed  = 0;

            Name = $"Animal Lore Contract: {m_AmountToTame} {selected.Name}";

            // Power‐scroll defaults
            m_PowerScrollSkill = SkillName.AnimalLore;
            m_PowerScrollValue = 0.0;
        }


        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                // Calculate PowerScrollValue based on player's current AnimalLore skill cap
                Skill animalLore = from.Skills[SkillName.AnimalLore];
                if (animalLore != null)
                {
                    double currentCap = animalLore.Cap;
                    PowerScrollValue = Math.Min(150.0, currentCap + 10.0);
                }
                else
                {
                    PowerScrollValue = 51.0; // Default value if skill is somehow missing
                }

                from.SendGump(new AnimalLoreContractGump(from, this));
            }
            else
            {
                from.SendLocalizedMessage(1047012); // This contract must be in your backpack to use it.
            }
        }

        public AnimalLoreContract(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version

            writer.Write(m_CreatureType);
            writer.Write(m_GoldReward);
            writer.Write(m_AmountToTame);
            writer.Write(m_AmountTamed);
            writer.Write((int)m_PowerScrollSkill);
            writer.Write(m_PowerScrollValue);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_CreatureType = reader.ReadInt();
            m_GoldReward = reader.ReadInt();
            m_AmountToTame = reader.ReadInt();
            m_AmountTamed = reader.ReadInt();

            if (version >= 1)
            {
                m_PowerScrollSkill = (SkillName)reader.ReadInt();
                m_PowerScrollValue = reader.ReadDouble();
            }

            LootType = LootType.Blessed;
        }
    }
}
