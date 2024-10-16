using Server.Commands;
using Server.Items;
using Server.Mobiles;
using System;

namespace Server.FarmingSystem
{
    public class SmokePipe : Item
    {
        private const int BURN_TIME = 10;//Minute Burn
        private int m_UsesRemaining = 0;
        private bool m_Smoking = false;
        private DateTime m_SmokeEnd;

        [CommandProperty(AccessLevel.GameMaster)]
        public int UsesRemaining { get { return m_UsesRemaining; } set { m_UsesRemaining = value; InvalidateProperties(); } }
        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime SmokeEnd
        {
            get { return m_SmokeEnd; }
            set { m_SmokeEnd = value; }
        }

        [Constructable]
        public SmokePipe()
            : base(0x97F)
        {
            Hue = 337;
            Weight = 1.0;
        }

        public override string DefaultName => "Smoke Pipe";

        public SmokePipe(Serial serial)
            : base(serial)
        {
        }
        public override void AddUsesRemainingProperties(ObjectPropertyList list)
        {
            list.Add(1060584, UsesRemaining.ToString()); // uses remaining: ~1_val~
        }
        public override void OnDoubleClick(Mobile from)
        {
            Item item = ((PlayerMobile)from).FindItemOnLayer(Layer.TwoHanded);

            if(item == null)
            {
                return;
            }

            if (item is Torch)
            {
                Torch torch = item as Torch;
                if (torch.Burning)
                {
                    if (IsChildOf(from.Backpack) || Parent == from)
                    {
                        if (m_UsesRemaining > 0)
                        {
                            from.PublicOverheadMessage(Network.MessageType.Emote, 1153, true, "PUFFffFfff PufffFfff");
                            Misc.SkillCheck.CheckSkill(from, SkillName.SpiritSpeak, 60, 140, Utility.Random(1, 4));

                            if (0.3 >= Utility.RandomDouble())
                                new ESound(from, 9);//Cough

                          
                          
                           
                        {
                              if (0.7 >= Utility.RandomDouble())
                               
                             {
                                 BuffInfo.AddBuff(from, new BuffInfo(BuffIcon.Warding, 1151395, 1116173, TimeSpan.FromMinutes(10),from,"25",false));
                                 new ESound(from, 9);//Cough
                                }
                                else  //FAILED
                                {
                                    from.ApplyPoison(from, Poison.Regular);
                                    new ESound(from, 26);//Puke
                                }

                                UsesRemaining--;
                            }
                        }
                        else
                        {
                            from.SendLocalizedMessage(1060847, string.Format("{0}", "Nothing in the pipe to smoke. Might want to fill it first."));
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                    }
                }
                else
                {
                    from.SendLocalizedMessage(1060847, string.Format("{0}", "The torch in your hand is not burning. Spark it up!"));
                }
            }
            else
            {
                from.SendLocalizedMessage(1060847, string.Format("{0}", "You do not have a torch in your hand."));
            }
        }

        private string GetRandomMessage()
        {
            string[] smokemessage = new string[]
            {
                "Puff Puff","Deep Inhale","Ouch my finger","This is some good stuff",
                "You should try this"
            };

            return smokemessage[Utility.Random(0,smokemessage.Length)]; 
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write(m_UsesRemaining);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
            m_UsesRemaining = reader.ReadInt();
        }
    }
}
