using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Qin Shi Huang")]
    public class QinShiHuang : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public QinShiHuang() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Qin Shi Huang";
            Body = 0x190; // Human male body

            // Stats
            SetStr(130);
            SetDex(70);
            SetInt(80);
            SetHits(90);

            // Appearance
            AddItem(new Robe() { Hue = 1160 });
            AddItem(new Cap() { Hue = 1160 });
            AddItem(new Sandals() { Hue = 1160 });
            AddItem(new Halberd() { Name = "Qin's Glaive" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public QinShiHuang(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Qin Shi Huang, the First Emperor of China. What knowledge do you seek from the annals of history?");

            greeting.AddOption("Tell me about the Qin Dynasty.",
                player => true,
                player =>
                {
                    DialogueModule qinDynastyModule = new DialogueModule("The Qin Dynasty was the first imperial dynasty of China, lasting from 221 to 206 BC. It was known for unifying the various warring states and creating a centralized bureaucracy. Would you like to know more about its achievements or its fall?");
                    qinDynastyModule.AddOption("What were the achievements of the Qin Dynasty?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule achievementsModule = new DialogueModule("The Qin Dynasty is credited with several monumental achievements:\n\n1. **Standardization of Measures**: We standardized weights, measures, and even the writing system to unify the diverse cultures within China.\n2. **Great Wall Construction**: Initiated the construction of the Great Wall to protect against northern invaders.\n3. **Terracotta Army**: I ordered the creation of my terracotta army to protect me in the afterlife. Each soldier is uniquely crafted.\n4. **Legal Reforms**: Implemented a unified code of laws that eliminated the feudal system.\n\nWhich achievement intrigues you the most?");
                            achievementsModule.AddOption("Tell me more about the Great Wall.",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule wallModule = new DialogueModule("The Great Wall of China stretches over 13,000 miles! It was built to protect the Chinese states from invasions by nomadic tribes. Construction involved forced labor and innovative engineering. Many sections are still standing today. Would you like to know about its history or the myths surrounding it?");
                                    wallModule.AddOption("What are the myths surrounding the Great Wall?",
                                        plb => true,
                                        plb =>
                                        {
                                            DialogueModule mythsModule = new DialogueModule("There are many legends about the Great Wall, including tales of it being visible from space or that it is haunted by the spirits of workers who died during its construction. It symbolizes strength and resilience. Do you believe in such legends?");
                                            mythsModule.AddOption("I find legends fascinating!",
                                                plc => true,
                                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                                            mythsModule.AddOption("Legends often hold truths.",
                                                plc => true,
                                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                                            plb.SendGump(new DialogueGump(plb, mythsModule));
                                        });
                                    pla.SendGump(new DialogueGump(pla, wallModule));
                                });
                            achievementsModule.AddOption("What about the Terracotta Army?",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule terracottaModule = new DialogueModule("The Terracotta Army was discovered in 1974 by farmers digging a well near my tomb. It consists of over 8,000 soldiers, each varying in size and expression, meant to guard me in the afterlife. Would you like to hear about their construction or their significance?");
                                    terracottaModule.AddOption("How were they constructed?",
                                        plb => true,
                                        plb =>
                                        {
                                            DialogueModule constructionModule = new DialogueModule("The soldiers were made using local clay and fired in kilns. Artisans crafted each piece meticulously, reflecting the rank and position of each soldier. They were then painted in vibrant colors, which faded over time. The sheer scale of this project is a testament to the craftsmanship of the era.");
                                            constructionModule.AddOption("Incredible! What about their significance?",
                                                plc => true,
                                                plc =>
                                                {
                                                    DialogueModule significanceModule = new DialogueModule("The Terracotta Army symbolizes my power and my belief in the afterlife. It reflects the importance of protecting one's soul and legacy. Would you like to learn about other significant artifacts from my era?");
                                                    significanceModule.AddOption("Yes, please share more artifacts.",
                                                        pld => true,
                                                        pld => pld.SendGump(new DialogueGump(pld, CreateArtifactModule())));
                                                    significanceModule.AddOption("Maybe later.",
                                                        pld => true,
                                                        pld => pld.SendGump(new DialogueGump(pld, CreateGreetingModule())));
                                                    plc.SendGump(new DialogueGump(plc, significanceModule));
                                                });
                                            plb.SendGump(new DialogueGump(plb, constructionModule));
                                        });
                                    pla.SendGump(new DialogueGump(pla, terracottaModule));
                                });
                            qinDynastyModule.AddOption("What led to the fall of the Qin Dynasty?",
                                plq => true,
                                plq => 
                                {
                                    DialogueModule fallModule = new DialogueModule("The Qin Dynasty fell due to several factors: harsh laws, heavy taxation, forced labor for monumental projects, and widespread resentment. After my death, the dynasty quickly descended into chaos. The Han Dynasty eventually rose from the ashes. Would you like to learn more about the Han Dynasty?");
                                    fallModule.AddOption("Yes, tell me about the Han Dynasty.",
                                        plw => true,
                                        plw => pl.SendGump(new DialogueGump(pl, CreateHanDynastyModule())));
                                    fallModule.AddOption("What about other dynasties?",
                                        ple => true,
                                        ple =>
                                        {
                                            DialogueModule otherDynastiesModule = new DialogueModule("China has a rich history with many dynasties, each contributing to its legacy. The Sui, Tang, Song, Yuan, Ming, and Qing dynasties all left their marks. Which dynasty intrigues you the most?");
                                            otherDynastiesModule.AddOption("Tell me about the Tang Dynasty.",
                                                p => true,
                                                p => p.SendGump(new DialogueGump(p, CreateTangDynastyModule())));
                                            otherDynastiesModule.AddOption("What about the Ming Dynasty?",
                                                p => true,
                                                p => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                                            pl.SendGump(new DialogueGump(pl, otherDynastiesModule));
                                        });
                                    player.SendGump(new DialogueGump(player, fallModule));
                                });
                            player.SendGump(new DialogueGump(player, achievementsModule));
                        });
                    player.SendGump(new DialogueGump(player, qinDynastyModule));
                });

            greeting.AddOption("What do you know about the Silk Road?",
                player => true,
                player =>
                {
                    DialogueModule silkRoadModule = new DialogueModule("The Silk Road was a network of trade routes that connected China with the Mediterranean, facilitating trade and cultural exchange. It was named for the lucrative silk trade that was carried along its length. Would you like to know about its importance or the cultures it connected?");
                    silkRoadModule.AddOption("Tell me about its importance.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule importanceModule = new DialogueModule("The Silk Road was crucial for the economic and cultural interactions between the East and West. It allowed for the exchange of goods, ideas, and technologies, enriching both sides. Would you like to learn about the goods traded or the historical figures involved?");
                            importanceModule.AddOption("What goods were traded?",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule goodsModule = new DialogueModule("Silk, spices, precious metals, and textiles were commonly traded. Additionally, cultural items like art and literature made their way along these routes, impacting civilizations. Are you curious about any specific trade items?");
                                    goodsModule.AddOption("Tell me more about silk.",
                                        plb => true,
                                        plb =>
                                        {
                                            DialogueModule silkDetailsModule = new DialogueModule("Silk is made from the cocoons of silkworms, primarily the Bombyx mori. It was a highly valued commodity due to its beauty and softness. The secret of silk production was a closely guarded secret for centuries. Would you like to know how it was produced?");
                                            silkDetailsModule.AddOption("Yes, how was silk produced?",
                                                plc => true,
                                                plc =>
                                                {
                                                    DialogueModule productionModule = new DialogueModule("The process involves cultivating silkworms, harvesting the cocoons, and then spinning the silk threads. It was labor-intensive and required great skill. The result was a luxurious fabric sought after by nobility across the world.");
                                                    plc.SendGump(new DialogueGump(plc, productionModule));
                                                });
                                            plb.SendGump(new DialogueGump(plb, silkDetailsModule));
                                        });
                                    goodsModule.AddOption("What about spices?",
                                        plb => true,
                                        plb =>
                                        {
                                            DialogueModule spicesModule = new DialogueModule("Spices like cinnamon, pepper, and saffron were highly prized for their flavor and preservative qualities. They played significant roles in culinary traditions and were often used as currency.");
                                            plb.SendGump(new DialogueGump(plb, spicesModule));
                                        });
                                    pla.SendGump(new DialogueGump(pla, goodsModule));
                                });
                            importanceModule.AddOption("Who were the historical figures involved?",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule figuresModule = new DialogueModule("Many figures played crucial roles in the Silk Road's history, such as Zhang Qian, a Han diplomat who helped establish trade relations with Central Asia, and Marco Polo, whose travels introduced Europeans to the wonders of the East. Would you like to learn about any specific figure?");
                                    figuresModule.AddOption("Tell me about Zhang Qian.",
                                        plc => true,
                                        plc => 
                                        {
                                            DialogueModule zhangModule = new DialogueModule("Zhang Qian was a diplomat and explorer during the Han Dynasty. His journeys helped open the Silk Road, establishing crucial trade routes and cultural exchanges. His adventures contributed greatly to the development of the Silk Road.");
                                            plc.SendGump(new DialogueGump(plc, zhangModule));
                                        });
                                    figuresModule.AddOption("What about Marco Polo?",
                                        plc => true,
                                        plc => 
                                        {
                                            DialogueModule marcoModule = new DialogueModule("Marco Polo was a Venetian merchant whose travels to China during the Yuan Dynasty were documented in 'The Travels of Marco Polo.' His accounts provided Europeans with valuable insights into Asian culture, geography, and trade.");
                                            plc.SendGump(new DialogueGump(plc, marcoModule));
                                        });
                                    pla.SendGump(new DialogueGump(pla, figuresModule));
                                });
                            player.SendGump(new DialogueGump(player, importanceModule));
                        });
                    silkRoadModule.AddOption("What cultures were connected?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule culturesModule = new DialogueModule("The Silk Road connected diverse cultures, including the Chinese, Persians, Arabs, Indians, and Europeans. This exchange enriched cultures through the sharing of religion, art, and technology. Would you like to explore specific cultural exchanges or events?");
                            culturesModule.AddOption("Tell me about cultural exchanges.",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule exchangeModule = new DialogueModule("The Silk Road facilitated exchanges such as Buddhism spreading from India to China, and Chinese inventions like paper reaching the West. This blending of cultures shaped civilizations for centuries. Are you interested in any specific cultural aspect?");
                                    exchangeModule.AddOption("What about Buddhism?",
                                        plb => true,
                                        plb =>
                                        {
                                            DialogueModule buddhismModule = new DialogueModule("Buddhism spread along the Silk Road, brought by monks and traders. It blended with local beliefs and practices, influencing art and philosophy. Would you like to learn more about its impact in China?");
                                            buddhismModule.AddOption("Yes, how did it impact China?",
                                                plc => true,
                                                plc =>
                                                {
                                                    DialogueModule impactModule = new DialogueModule("Buddhism became deeply rooted in Chinese society, influencing philosophy, literature, and art. It led to the development of unique Chinese Buddhist schools and practices, profoundly shaping spiritual life.");
                                                    plc.SendGump(new DialogueGump(plc, impactModule));
                                                });
                                            plb.SendGump(new DialogueGump(plb, buddhismModule));
                                        });
                                    exchangeModule.AddOption("What about Chinese inventions?",
                                        plb => true,
                                        plb =>
                                        {
                                            DialogueModule inventionsModule = new DialogueModule("Chinese inventions such as gunpowder, papermaking, and the compass traveled along the Silk Road, impacting societies globally. Would you like to learn about any specific invention?");
                                            inventionsModule.AddOption("Tell me about gunpowder.",
                                                plc => true,
                                                plc =>
                                                {
                                                    DialogueModule gunpowderModule = new DialogueModule("Gunpowder was invented during the Tang Dynasty. Initially used for fireworks and religious ceremonies, it later revolutionized warfare. Its impact on history cannot be understated!");
                                                    plc.SendGump(new DialogueGump(plc, gunpowderModule));
                                                });
                                            inventionsModule.AddOption("What about papermaking?",
                                                plc => true,
                                                plc =>
                                                {
                                                    DialogueModule paperModule = new DialogueModule("Papermaking was developed during the Han Dynasty and spread throughout the world via the Silk Road. This invention revolutionized communication, education, and record-keeping.");
                                                    plc.SendGump(new DialogueGump(plc, paperModule));
                                                });
                                            pl.SendGump(new DialogueGump(pl, inventionsModule));
                                        });
                                    pla.SendGump(new DialogueGump(pla, exchangeModule));
                                });
                            pl.SendGump(new DialogueGump(pl, culturesModule));
                        });
                    player.SendGump(new DialogueGump(player, silkRoadModule));
                });

            greeting.AddOption("What can you tell me about Chinese philosophy?",
                player => true,
                player =>
                {
                    DialogueModule philosophyModule = new DialogueModule("Chinese philosophy encompasses a wide range of schools, including Confucianism, Daoism, and Legalism. Each offers unique insights into ethics, governance, and the nature of reality. Would you like to explore a specific philosophy?");
                    philosophyModule.AddOption("Tell me about Confucianism.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule confucianismModule = new DialogueModule("Confucianism, founded by Confucius, emphasizes ethics, morality, and social harmony. It advocates for respect for elders, education, and virtuous leadership. Would you like to know about its influence on Chinese society?");
                            confucianismModule.AddOption("Yes, how did it influence society?",
                                plc => true,
                                plc =>
                                {
                                    DialogueModule influenceModule = new DialogueModule("Confucianism has shaped Chinese culture and values for centuries. It is central to family life, education, and government, advocating for a society rooted in harmony and respect.");
                                    plc.SendGump(new DialogueGump(plc, influenceModule));
                                });
                            pl.SendGump(new DialogueGump(pl, confucianismModule));
                        });
                    philosophyModule.AddOption("What about Daoism?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule daoismModule = new DialogueModule("Daoism, attributed to Laozi, emphasizes living in harmony with the Dao, or 'the Way.' It values simplicity, spontaneity, and nature. Would you like to learn about its practices or texts?");
                            daoismModule.AddOption("Tell me about its practices.",
                                plc => true,
                                plc =>
                                {
                                    DialogueModule practicesModule = new DialogueModule("Daoist practices include meditation, breathing exercises, and rituals aimed at harmonizing with nature. It encourages followers to align themselves with the rhythms of the universe.");
                                    plc.SendGump(new DialogueGump(plc, practicesModule));
                                });
                            daoismModule.AddOption("What about its texts?",
                                plc => true,
                                plc =>
                                {
                                    DialogueModule textsModule = new DialogueModule("The 'Tao Te Ching' is the foundational text of Daoism, exploring concepts of simplicity and naturalness. The 'Zhuangzi' expands on these ideas with philosophical anecdotes and parables.");
                                    plc.SendGump(new DialogueGump(plc, textsModule));
                                });
                            pl.SendGump(new DialogueGump(pl, daoismModule));
                        });
                    philosophyModule.AddOption("What is Legalism?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule legalismModule = new DialogueModule("Legalism is a strict philosophy emphasizing law and order. It advocates for a strong central authority and the enforcement of harsh laws to maintain social stability. Would you like to know about its role in the Qin Dynasty?");
                            legalismModule.AddOption("Yes, how did it influence the Qin Dynasty?",
                                plc => true,
                                plc =>
                                {
                                    DialogueModule legalismInfluenceModule = new DialogueModule("Legalism played a crucial role in my rule, shaping policies that centralized power and enforced order through strict laws. While effective, it also led to widespread resentment among the populace.");
                                    plc.SendGump(new DialogueGump(plc, legalismInfluenceModule));
                                });
                            pl.SendGump(new DialogueGump(pl, legalismModule));
                        });
                    player.SendGump(new DialogueGump(player, philosophyModule));
                });

            return greeting;
        }

        private DialogueModule CreateArtifactModule()
        {
            DialogueModule artifactModule = new DialogueModule("My collection contains many fascinating artifacts, such as ancient scrolls, jade carvings, and ceremonial weapons. Each tells a story from the rich tapestry of our history. Would you like to hear about a specific artifact?");
            artifactModule.AddOption("Tell me about jade carvings.",
                pl => true,
                pl =>
                {
                    DialogueModule jadeModule = new DialogueModule("Jade carvings have been treasured in Chinese culture for millennia. They symbolize purity and moral integrity. Traditionally, jade is used in rituals and is believed to possess protective qualities. Would you like to learn about a specific jade artifact?");
                    jadeModule.AddOption("What about the Heirloom Jade?",
                        plc => true,
                        plc =>
                        {
                            DialogueModule heirloomModule = new DialogueModule("The Heirloom Jade is a remarkable piece, passed down through generations. It is said to grant wisdom to its possessor. Many seek to uncover its secrets. Are you interested in its history?");
                            heirloomModule.AddOption("Yes, how did it come to be?",
                                plq => true,
                                plq =>
                                {
                                    DialogueModule historyModule = new DialogueModule("The Heirloom Jade was once owned by a wise emperor who used it to guide his rule. It was later hidden during times of turmoil and rediscovered by a scholar seeking knowledge. Its legacy continues to inspire many.");
                                    plq.SendGump(new DialogueGump(plq, historyModule));
                                });
                            plc.SendGump(new DialogueGump(plc, heirloomModule));
                        });
                    pl.SendGump(new DialogueGump(pl, jadeModule));
                });
            artifactModule.AddOption("What about ceremonial weapons?",
                pl => true,
                pl =>
                {
                    DialogueModule weaponModule = new DialogueModule("Ceremonial weapons symbolize power and authority. They were often used in rituals to honor ancestors and seek blessings from the gods. Each weapon has its unique story. Are you curious about a particular weapon?");
                    weaponModule.AddOption("Tell me about the Dragon Sword.",
                        plc => true,
                        plc =>
                        {
                            DialogueModule swordModule = new DialogueModule("The Dragon Sword is said to embody the spirit of the dragon, a symbol of strength and protection. It was once wielded by a legendary warrior. Many believe it grants courage to its bearer.");
                            plc.SendGump(new DialogueGump(plc, swordModule));
                        });
                    pl.SendGump(new DialogueGump(pl, weaponModule));
                });
            return artifactModule;
        }

        private DialogueModule CreateHanDynastyModule()
        {
            DialogueModule hanModule = new DialogueModule("The Han Dynasty, lasting from 206 BC to 220 AD, is considered a golden age in Chinese history. It is known for significant developments in culture, technology, and governance. Would you like to learn about its achievements or significant figures?");
            hanModule.AddOption("What were its achievements?",
                pl => true,
                pl =>
                {
                    DialogueModule achievementsModule = new DialogueModule("The Han Dynasty saw advancements such as the invention of paper, the seismograph, and the expansion of the Silk Road. It also established the civil service system, promoting merit-based appointments. Which achievement would you like to know more about?");
                    achievementsModule.AddOption("Tell me about the invention of paper.",
                        plc => true,
                        plc =>
                        {
                            DialogueModule paperModule = new DialogueModule("Paper was invented during the Han Dynasty by Cai Lun. It revolutionized writing and record-keeping, making knowledge more accessible. The invention was a turning point for education and literature.");
                            plc.SendGump(new DialogueGump(plc, paperModule));
                        });
                    achievementsModule.AddOption("What about the civil service system?",
                        plc => true,
                        plc =>
                        {
                            DialogueModule civilServiceModule = new DialogueModule("The civil service system established by the Han emphasized the importance of education and merit. It created a bureaucracy that helped govern effectively and efficiently. This system influenced many future administrations.");
                            plc.SendGump(new DialogueGump(plc, civilServiceModule));
                        });
                    pl.SendGump(new DialogueGump(pl, achievementsModule));
                });
            hanModule.AddOption("Who were the significant figures?",
                pl => true,
                pl =>
                {
                    DialogueModule figuresModule = new DialogueModule("Key figures of the Han Dynasty include Emperor Wu, who expanded the empire's territory, and Zhang Qian, known for his explorations that opened the Silk Road. Would you like to hear about a specific figure?");
                    figuresModule.AddOption("Tell me about Emperor Wu.",
                        plc => true,
                        plc =>
                        {
                            DialogueModule wuModule = new DialogueModule("Emperor Wu, also known as Wu of Han, reigned from 141 to 87 BC. He is celebrated for his military conquests and significant territorial expansion. He promoted Confucianism and established it as the state ideology.");
                            plc.SendGump(new DialogueGump(plc, wuModule));
                        });
                    figuresModule.AddOption("What about Zhang Qian?",
                        plc => true,
                        plc =>
                        {
                            DialogueModule qianModule = new DialogueModule("Zhang Qian was an explorer and diplomat whose journeys helped establish trade routes that became known as the Silk Road. His reports introduced the Han Dynasty to Central Asia, leading to significant cultural exchanges.");
                            plc.SendGump(new DialogueGump(plc, qianModule));
                        });
                    pl.SendGump(new DialogueGump(pl, figuresModule));
                });
            return hanModule;
        }

        private DialogueModule CreateTangDynastyModule()
        {
            DialogueModule tangModule = new DialogueModule("The Tang Dynasty, which lasted from 618 to 907 AD, is often regarded as a high point in Chinese civilization, particularly known for its cultural and artistic achievements. Would you like to learn about its culture, politics, or significant figures?");
            tangModule.AddOption("Tell me about its culture.",
                pl => true,
                pl =>
                {
                    DialogueModule cultureModule = new DialogueModule("Tang culture flourished with advancements in poetry, painting, and music. The dynasty is often referred to as the 'Golden Age of Chinese Poetry.' Would you like to know about specific poets or artistic techniques?");
                    cultureModule.AddOption("Tell me about Li Bai.",
                        plc => true,
                        plc =>
                        {
                            DialogueModule liBaiModule = new DialogueModule("Li Bai, known as 'the Immortal Poet,' was famous for his romantic and imaginative poetry. His works often reflect nature and personal emotions. Would you like to hear one of his famous poems?");
                            liBaiModule.AddOption("Yes, please share a poem.",
                                plq => true,
                                plq => 
                                {
                                    DialogueModule poemModule = new DialogueModule("'A Quiet Night Thought' is one of his most cherished poems, reflecting on homesickness. It goes:\n\n'Before my bed, the moonlight glows,\nI suspect it is frost on the ground.\nRaising my head, I gaze at the bright moon,\nLowering my head, I think of my hometown.'");
                                    plq.SendGump(new DialogueGump(plq, poemModule));
                                });
                            plc.SendGump(new DialogueGump(plc, liBaiModule));
                        });
                    cultureModule.AddOption("What about painting?",
                        plc => true,
                        plc =>
                        {
                            DialogueModule paintingModule = new DialogueModule("Tang painting is characterized by its boldness and attention to detail. Artists often depicted landscapes, flowers, and animals, reflecting the beauty of nature. Would you like to learn about any famous painters?");
                            paintingModule.AddOption("Tell me about Wu Daozi.",
                                plq => true,
                                plq =>
                                {
                                    DialogueModule wuDaoziModule = new DialogueModule("Wu Daozi is known as the 'Sage of Painting.' His works focused on Buddhist themes and are celebrated for their spiritual depth. His ability to convey emotions through brushstrokes was unparalleled.");
                                    plq.SendGump(new DialogueGump(plq, wuDaoziModule));
                                });
                            plc.SendGump(new DialogueGump(plc, paintingModule));
                        });
                    pl.SendGump(new DialogueGump(pl, cultureModule));
                });
            tangModule.AddOption("What about its politics?",
                pl => true,
                pl =>
                {
                    DialogueModule politicsModule = new DialogueModule("The Tang Dynasty established a strong central government with a well-defined bureaucracy. This era saw the rise of the civil service examination system, promoting talent and education. Would you like to know about the examination process?");
                    politicsModule.AddOption("Yes, how did it work?",
                        plc => true,
                        plc =>
                        {
                            DialogueModule examModule = new DialogueModule("The civil service examination tested candidates on Confucian texts, poetry, and governance. Those who succeeded secured prestigious positions within the government, allowing for a meritocratic system that emphasized education.");
                            plc.SendGump(new DialogueGump(plc, examModule));
                        });
                    pl.SendGump(new DialogueGump(pl, politicsModule));
                });
            tangModule.AddOption("Who were the significant figures?",
                pl => true,
                pl =>
                {
                    DialogueModule figuresModule = new DialogueModule("Prominent figures of the Tang Dynasty include Empress Wu Zetian, the only female emperor in Chinese history, and Xuanzang, the monk who traveled to India to retrieve Buddhist texts. Would you like to learn about a specific figure?");
                    figuresModule.AddOption("Tell me about Wu Zetian.",
                        plc => true,
                        plc =>
                        {
                            DialogueModule wuZetianModule = new DialogueModule("Wu Zetian rose to power in a male-dominated society and became the only woman to rule China in her own right. Her reign is marked by significant political and social reforms. Would you like to hear more about her policies?");
                            wuZetianModule.AddOption("Yes, tell me about her policies.",
                                plq => true,
                                plq =>
                                {
                                    DialogueModule policiesModule = new DialogueModule("Wu Zetian implemented policies promoting meritocracy, encouraging the selection of officials based on talent rather than birthright. She also supported the arts and religious tolerance.");
                                    plq.SendGump(new DialogueGump(plq, policiesModule));
                                });
                            plc.SendGump(new DialogueGump(plc, wuZetianModule));
                        });
                    figuresModule.AddOption("What about Xuanzang?",
                        plc => true,
                        plc =>
                        {
                            DialogueModule xuanzangModule = new DialogueModule("Xuanzang was a Buddhist monk who traveled to India to obtain sacred texts. His journey inspired the classic novel 'Journey to the West.' He is celebrated for his dedication to spreading Buddhism.");
                            plc.SendGump(new DialogueGump(plc, xuanzangModule));
                        });
                    pl.SendGump(new DialogueGump(pl, figuresModule));
                });
            return tangModule;
        }

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
