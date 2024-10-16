using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.FarmingSystem
{
    public class Tobacco : Item
    {
        [Constructable]
        public Tobacco()
            : this(1)
        {
        }

        [Constructable]
        public Tobacco(int amount)
            : base(0x0DF8)
        {
            Stackable = true;
            Weight = 1.0;
            Amount = amount;
            Hue = 448;
           
        }

        public override string DefaultName => "Tobacco";

        public Tobacco(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                from.Target = new TobaccoTarget(this);
                from.SendLocalizedMessage(1060847, string.Format("{0}", "Target what you want to use this on."));
            }
            else
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
        }


        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }

        private class TobaccoTarget : Target
        {
            private Tobacco tobacco;

            public TobaccoTarget(Tobacco tobacco):base(0,false,TargetFlags.None)
            {
                this.tobacco = tobacco;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if(targeted is PlayerMobile)
                {
                    if (((Mobile)targeted == from))
                    {
                        if (tobacco.Amount > 1)
                            tobacco.Amount -= 1;
                        else
                            tobacco.Delete();

                        if (from.Hunger < 20)
                            from.Hunger++;

                        from.SendLocalizedMessage(1060847, string.Format("{0}", "You eat the tobacco."));


                        if (0.75 >= Utility.RandomDouble() && from.Hunger >= 20)
                            new Commands.ESound(from, 26);
                        else if(0.25 >= Utility.RandomDouble())
                            new Commands.ESound(from, 26);
                    }
                    else
                    {
                        from.SendLocalizedMessage(1060847, string.Format("{0}", "This will not work on that."));
                    }
                }
                else if(targeted is SmokePipe)
                {
                    SmokePipe pipe = targeted as SmokePipe;

                    if (pipe.UsesRemaining == 0)
                    {
                        pipe.UsesRemaining = 5;

                        if (tobacco.Amount > 1)
                            tobacco.Amount -= 1;
                        else
                            tobacco.Delete();

                        from.SendLocalizedMessage(1060847, string.Format("{0}", "You fill the pipe."));
                    }
                    else
                        from.SendLocalizedMessage(1060847, string.Format("{0}", "The pipe still has something in it. Empty it first."));
                }
                else
                {
                    from.SendLocalizedMessage(1060847, string.Format("{0}", "This will not work on that."));
                }
            }
        }
    }
}
