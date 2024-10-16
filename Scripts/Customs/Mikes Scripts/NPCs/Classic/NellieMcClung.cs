using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Nellie McClung")]
    public class NellieMcClung : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public NellieMcClung() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Nellie McClung";
            Body = 0x191; // Human female body

            // Stats
            Str = 75;
            Dex = 80;
            Int = 110;
            Hits = 70;

            // Appearance
            AddItem(new Kilt() { Hue = 1115 });
            AddItem(new FancyShirt() { Hue = 1120 });
            AddItem(new Boots() { Hue = 1155 });
            AddItem(new Bonnet() { Hue = 1132 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Nellie McClung, a proud Canadian. How can I assist you today?");

            greeting.AddOption("What can you tell me about Canada’s history?",
                player => true,
                player =>
                {
                    DialogueModule historyModule = new DialogueModule("Canada has a rich tapestry of history woven with diverse cultures and events. Would you like to learn about Indigenous history, the fur trade, or the Confederation?");
                    historyModule.AddOption("Tell me about Indigenous history.",
                        p => true,
                        p =>
                        {
                            DialogueModule indigenousModule = new DialogueModule("Before European contact, the land was inhabited by numerous Indigenous peoples with rich cultures and traditions. Their stories are vital to understanding Canada. Would you like to hear about a specific nation?");
                            indigenousModule.AddOption("What about the First Nations?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule firstNationsModule = new DialogueModule("The First Nations are diverse, with over 600 recognized groups. Each has its own unique culture, language, and history. For instance, the Haudenosaunee Confederacy played a significant role in governance and diplomacy.");
                                    firstNationsModule.AddOption("What is the Haudenosaunee Confederacy?",
                                        p2 => true,
                                        p2 =>
                                        {
                                            p2.SendGump(new DialogueGump(p2, new DialogueModule("The Haudenosaunee Confederacy, also known as the Iroquois Confederacy, is a political alliance of six nations: Mohawk, Oneida, Onondaga, Cayuga, Seneca, and Tuscarora. They are known for their democratic principles that influenced the U.S. Constitution.")));
                                        });
                                    firstNationsModule.AddOption("What about the Métis?",
                                        p2 => true,
                                        p2 =>
                                        {
                                            p2.SendGump(new DialogueGump(p2, new DialogueModule("The Métis are a distinct group formed from the intermarriage of Indigenous peoples and European settlers. They have their own unique culture, traditions, and language, notably Michif.")));
                                        });
                                    pl.SendGump(new DialogueGump(pl, firstNationsModule));
                                });
                            indigenousModule.AddOption("Tell me about the Inuit.",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule inuitModule = new DialogueModule("The Inuit are Indigenous peoples who primarily inhabit the Arctic regions. Their culture is deeply connected to the land and sea, and they have a rich tradition of storytelling and art. Would you like to know about their traditional practices?");
                                    inuitModule.AddOption("What are their traditional practices?",
                                        p2 => true,
                                        p2 =>
                                        {
                                            p2.SendGump(new DialogueGump(p2, new DialogueModule("Inuit traditions include hunting seals and whales, building igloos, and creating intricate carvings and throat singing. Their lifestyle reflects the harsh Arctic environment they inhabit.")));
                                        });
                                    pl.SendGump(new DialogueGump(pl, inuitModule));
                                });
                            player.SendGump(new DialogueGump(player, indigenousModule));
                        });

                    historyModule.AddOption("Tell me about the fur trade.",
                        p => true,
                        p =>
                        {
                            DialogueModule furTradeModule = new DialogueModule("The fur trade was a major driver of early Canadian economy, involving European powers and Indigenous peoples. It significantly shaped the cultural and economic landscape of Canada.");
                            furTradeModule.AddOption("How did it start?",
                                pl => true,
                                pl =>
                                {
                                    pl.SendGump(new DialogueGump(pl, new DialogueModule("The fur trade began in the early 17th century when French traders sought beaver pelts to meet European demand for fashion. This led to the establishment of trading posts and alliances with Indigenous peoples.")));
                                });
                            furTradeModule.AddOption("What impact did it have?",
                                pl => true,
                                pl =>
                                {
                                    pl.SendGump(new DialogueGump(pl, new DialogueModule("The fur trade led to the development of major cities, such as Montreal, and transformed Indigenous lifestyles. However, it also resulted in exploitation and significant social changes.")));
                                });
                            player.SendGump(new DialogueGump(player, furTradeModule));
                        });

                    historyModule.AddOption("What about the Confederation?",
                        p => true,
                        p =>
                        {
                            DialogueModule confederationModule = new DialogueModule("The Confederation of Canada in 1867 marked the union of the British North American colonies into a single Dominion. This event laid the foundation for modern Canada.");
                            confederationModule.AddOption("Who were the key figures?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule figuresModule = new DialogueModule("Key figures included Sir John A. Macdonald, the first Prime Minister, and George-Étienne Cartier, who played crucial roles in unifying the provinces. Their leadership helped shape the new nation.");
                                    figuresModule.AddOption("What challenges did they face?",
                                        p2 => true,
                                        p2 =>
                                        {
                                            p2.SendGump(new DialogueGump(p2, new DialogueModule("The leaders faced challenges such as regional disparities, differing interests, and the inclusion of the western territories. Overcoming these hurdles was vital for establishing a cohesive nation.")));
                                        });
                                    pl.SendGump(new DialogueGump(pl, figuresModule));
                                });
                            confederationModule.AddOption("What were the implications of Confederation?",
                                pl => true,
                                pl =>
                                {
                                    pl.SendGump(new DialogueGump(pl, new DialogueModule("Confederation led to greater autonomy for provinces and the establishment of a parliamentary system. It also paved the way for further expansion and the creation of a transcontinental railway.")));
                                });
                            player.SendGump(new DialogueGump(player, confederationModule));
                        });

                    player.SendGump(new DialogueGump(player, historyModule));
                });

            greeting.AddOption("What is your role in Canadian history?",
                player => true,
                player =>
                {
                    DialogueModule roleModule = new DialogueModule("As a writer and suffragist, I campaigned for women's rights and pushed for social reform in Canada. I believe that sharing stories and advocating for justice is vital in shaping our nation.");
                    roleModule.AddOption("What specifically did you campaign for?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("I fought for the right of women to vote and to hold public office. I believed that women should have a voice in the decisions that affect their lives.")));
                        });
                    roleModule.AddOption("Who were your influences?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Many women inspired me, including Susan B. Anthony and Emmeline Pankhurst. Their courage and dedication to the cause of women's rights motivated me to advocate for change in Canada.")));
                        });
                    player.SendGump(new DialogueGump(player, roleModule));
                });

            greeting.AddOption("Can you tell me about Canadian culture?",
                player => true,
                player =>
                {
                    DialogueModule cultureModule = new DialogueModule("Canadian culture is a blend of Indigenous, French, British, and immigrant influences. It is reflected in our arts, music, and culinary traditions.");
                    cultureModule.AddOption("What are some famous Canadian artists?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Artists like Emily Carr, known for her landscapes and Indigenous themes, and the Group of Seven, who captured Canada's natural beauty, are iconic in Canadian art.")));
                        });
                    cultureModule.AddOption("What about Canadian music?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Canada has produced many renowned musicians, including Leonard Cohen, Joni Mitchell, and Drake. Our music scene is diverse, encompassing folk, rock, and pop genres.")));
                        });
                    player.SendGump(new DialogueGump(player, cultureModule));
                });

            greeting.AddOption("What are your thoughts on the future of Canada?",
                player => true,
                player =>
                {
                    DialogueModule futureModule = new DialogueModule("I believe Canada has the potential to continue evolving into a more inclusive and equitable society. We must learn from our past and work towards a brighter future for all.");
                    futureModule.AddOption("How can we achieve that?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("By engaging in conversations about social justice, supporting diverse voices, and advocating for change at all levels of government. Every action, no matter how small, contributes to progress.")));
                        });
                    player.SendGump(new DialogueGump(player, futureModule));
                });

            return greeting;
        }

        public NellieMcClung(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastRewardTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime();
        }
    }
}
