using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a taekwondo master")]
    public class TaekwondoMaster : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between master speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public TaekwondoMaster() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Taekwondo Master";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Taekwondo Master";
            }

            Item robe = new Robe();
            robe.Hue = 1154; // White robe
            AddItem(robe);

            Item belt = new BodySash();
            belt.Hue = 1175; // Black belt
            AddItem(belt);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(hair);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(500, 800);
            SetDex(300, 400);
            SetInt(100, 150);

            SetHits(400, 600);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 50);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Anatomy, 90.1, 100.0);
            SetSkill(SkillName.MagicResist, 80.0, 90.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 95.1, 100.0);

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 40;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return false; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "Prepare to face my kicks!"); break;
                        case 1: this.Say(true, "You can't dodge my moves!"); break;
                        case 2: this.Say(true, "Feel the power of Taekwondo!"); break;
                        case 3: this.Say(true, "I will defeat you with my skill!"); break;
                    }
                    
                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(300, 400);
            AddLoot(LootPack.Rich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "My journey ends..."); break;
                case 1: this.Say(true, "You fought well..."); break;
            }

            PackItem(new BlackPearl(Utility.RandomMinMax(10, 20)));
        }

        public override int Damage(int amount, Mobile from)
        {
            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
            {
                if (Utility.RandomBool())
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "You think a hit like that can stop me?!"); break;
                        case 1: this.Say(true, "I have trained for this moment!"); break;
                        case 2: this.Say(true, "My resolve is unbreakable!"); break;
                        case 3: this.Say(true, "You will need more than that!"); break;
                    }
                    
                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }

            // High-flying kick area damage
            if (Utility.RandomDouble() < 0.25) // 25% chance to trigger
            {
                foreach (Mobile m in this.GetMobilesInRange(2)) // 2 tile radius
                {
                    if (m != this && m != from && this.CanBeHarmful(m))
                    {
                        this.DoHarmful(m);
                        m.Damage(Utility.RandomMinMax(10, 20), this); // Area damage
                        m.SendMessage("You are struck by a high-flying kick!");
                    }
                }
            }

            return base.Damage(amount, from);
        }

        public TaekwondoMaster(Serial serial) : base(serial)
        {
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
        }
    }
}
