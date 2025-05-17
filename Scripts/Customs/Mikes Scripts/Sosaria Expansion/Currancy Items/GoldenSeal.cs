using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
    public class GoldenSeal : Item
    {
        [Constructable]
        public GoldenSeal() : base(0x1F14) // you can change the itemID as desired
        {
            Name  = "Golden Seal";
            Hue   = 1154;       // golden hue
            Weight = 0.5;
        }

        public GoldenSeal(Serial serial) : base(serial)
        {
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Marks the treasure for a royal share.");
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack to use it.
                return;
            }

            from.SendMessage("Select the magic map you wish to empower.");
            from.Target = new SealTarget(this);
        }

        private class SealTarget : Target
        {
            private readonly GoldenSeal _seal;

            public SealTarget(GoldenSeal seal) : base(12, false, TargetFlags.None)
            {
                _seal = seal;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (_seal.Deleted)
                    return;

                if (targeted is MagicMapBase map)
                {
                    if (!map.IsChildOf(from.Backpack))
                    {
                        from.SendMessage("That map must be in your backpack to use the Golden Seal.");
                        return;
                    }

                    if (map.ChestLevel >= 5)
                    {
                        from.SendMessage("This map's treasure is already at the highest rarity.");
                        return;
                    }

                    map.ChestLevel += 1;
                    from.SendMessage($"The Golden Seal glows! Treasure Rarity is now Level {map.ChestLevel}.");

                    _seal.Delete();
                }
                else
                {
                    from.SendMessage("You can only use the Golden Seal on a magic map.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                if (!_seal.Deleted)
                    from.SendMessage("You decide not to use the Golden Seal.");
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
