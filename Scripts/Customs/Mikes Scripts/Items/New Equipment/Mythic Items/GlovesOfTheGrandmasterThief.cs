using System;
using Server;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
    public class GlovesOfTheGrandmasterThief : Item
    {
        [Constructable]
        public GlovesOfTheGrandmasterThief() : base(0x13C6)
        {
            Name = "Gloves of the Grandmaster Thief";
            Hue = 1157;
            Weight = 1.0;
        }

        public GlovesOfTheGrandmasterThief(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from is PlayerMobile)
            {
                PlayerMobile pm = (PlayerMobile)from;
                pm.SendMessage("Target the item you wish to steal.");
                pm.Target = new InternalTarget(this, pm.Skills[SkillName.Stealing].Base);
            }
        }

        private class InternalTarget : Target
        {
            private readonly GlovesOfTheGrandmasterThief m_Gloves;
            private readonly double m_StealingSkill;

            public InternalTarget(GlovesOfTheGrandmasterThief gloves, double stealingSkill)
                : base(1, false, TargetFlags.None)
            {
                m_Gloves = gloves;
                m_StealingSkill = stealingSkill;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item)
                {
                    Item targetItem = (Item)targeted;
                    Random rand = new Random();

                    if (!targetItem.Movable)
                    {
                        if (rand.Next(0, 100) < m_StealingSkill)
                        {
                            // Success
                            Item copiedItem = new Item(targetItem.ItemID);
                            copiedItem.Name = targetItem.Name;
                            copiedItem.Hue = targetItem.Hue;
                            copiedItem.Movable = true;

                            from.AddToBackpack(copiedItem);
                            from.SendMessage("You successfully stole the item!");

                            if (from.Skills[SkillName.Stealing].Base < 100.0)
                            {
                                from.Skills[SkillName.Stealing].Base += 1.0; // Increase skill by 1%
                            }
                        }
                        else
                        {
                            // Failure
                            from.SendMessage("You failed to steal the item.");
                            
                            if (rand.Next(0, 100) < 50)
                            {
                                // 50% chance to destroy the gloves
                                m_Gloves.Delete();
                            }
                        }
                    }
                    else
                    {
                        from.SendMessage("That item cannot be stolen.");
                    }
                }
                else
                {
                    from.SendMessage("You must target an item.");
                }
            }
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
