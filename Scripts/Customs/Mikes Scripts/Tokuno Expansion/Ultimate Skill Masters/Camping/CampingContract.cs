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
    public class CampingContract : Item
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
        public CampingContract(PlayerMobile player) : base(0x14EF)
        {
            Weight = 1.0;
            Movable = true;
            LootType = LootType.Blessed;

            // Filter creatures based on player's taming skill
            var validCreatures = AnimalTamingContractType.Get.Where(creatureType =>
            {
                try
                {
                    var creature = Activator.CreateInstance(creatureType.Type) as BaseCreature;
                    return creature != null && creature.Tamable &&
                           player.Skills[SkillName.AnimalTaming].Value >= creature.MinTameSkill;
                }
                catch
                {
                    return false;
                }
            }).ToList();

            if (validCreatures.Count > 0)
            {
                CreatureType = Utility.Random(validCreatures.Count);
                var selectedCreature = validCreatures[CreatureType];

                AmountToTame = Utility.RandomMinMax(5, 10); // Taming animals is quite time consuming, we shouldnt request too many animals
                GoldReward = AmountToTame * 500;
                Name = "Camping Contract: " + AmountToTame + " " + selectedCreature.Name;
                AmountTamed = 0;

                PowerScrollSkill = SkillName.Camping;
                PowerScrollValue = 0; // Placeholder
            }
            else
            {
                Name = "Camping Contract: No tamable creatures available";
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                // Calculate PowerScrollValue based on player's current Camping skill cap
                Skill Camping = from.Skills[SkillName.Camping];
                if (Camping != null)
                {
                    double currentCap = Camping.Cap;
                    PowerScrollValue = Math.Min(150.0, currentCap + 10.0);
                }
                else
                {
                    PowerScrollValue = 51.0; // Default value if skill is somehow missing
                }

                from.SendGump(new CampingContractGump(from, this));
            }
            else
            {
                from.SendLocalizedMessage(1047012); // This contract must be in your backpack to use it.
            }
        }

        public CampingContract(Serial serial) : base(serial) { }

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
