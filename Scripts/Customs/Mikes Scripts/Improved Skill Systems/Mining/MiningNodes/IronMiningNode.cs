using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
    public class IronMiningNode : Item
    {
        [Constructable]
        public IronMiningNode() : base(0x1363) // Set this to the item ID for a stone
        {
            Movable = false;
            Name = "Iron Mining Node";
        }

        public override void OnDoubleClick(Mobile from)
        {
            PlayerMobile pm = from as PlayerMobile;

            if (pm == null || !pm.Player)
            {
                from.SendMessage("Only players can mine this.");
                return;
            }

            if (from.InRange(this.GetWorldLocation(), 2))
            {
                Item pickaxe = from.FindItemOnLayer(Layer.OneHanded);

                if (pickaxe != null && (pickaxe is Pickaxe || pickaxe is SturdyPickaxe))
                {
                    if (pm.Skills.Mining.Value < 1)
                    {
                        from.SendMessage("You have no idea how to mine.");
                        return;
                    }

                    // Calculate delays and animation based on Mining skill
                    double skill = pm.Skills.Mining.Value;
                    int amount = Utility.Random(1, 5); // 1-5 iron ore
                    int delay = (int)(10 - (9 * (skill / 100))); // Longer delay for lower skill
                    int animations = 1 + (int)(skill / 20); // More animations for higher skill

                    from.SendMessage("You start mining the node.");
                    Timer.DelayCall(TimeSpan.FromSeconds(delay), () =>
                    {
                        for (int i = 0; i < animations; i++)
                        {
                            from.Animate(11, 5, 1, true, false, 0); // Mining animation
                            Timer.DelayCall(TimeSpan.FromSeconds(i * 1.6 + 0.9), () =>
                            {
                                from.PlaySound(0x125);
                            });
                        }

                        from.AddToBackpack(new IronOre(amount));
                        from.SendMessage("You mine some iron ore.");
                        this.Delete(); // Remove the node after mining
                    });
                }
                else
                {
                    from.SendMessage("You need to equip a pickaxe to mine this node.");
                }
            }
            else
            {
                from.SendMessage("You are too far away to mine that.");
            }
        }

        public IronMiningNode(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
