using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a beastmaster")]
    public class Beastmaster : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between beastmaster speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public Beastmaster() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Beastmaster";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Beastmaster";
            }

            Item robe = new Robe();
            AddItem(robe);

            Item boots = new Boots();
            AddItem(boots);

            Item staff = new GnarledStaff();
            AddItem(staff);
            staff.Movable = false;

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

            SetStr(700, 1000);
            SetDex(150, 250);
            SetInt(300, 400);

            SetHits(500, 800);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 25);
            SetDamageType(ResistanceType.Cold, 25);

            SetResistance(ResistanceType.Physical, 60, 75);
            SetResistance(ResistanceType.Fire, 50, 65);
            SetResistance(ResistanceType.Cold, 55, 70);
            SetResistance(ResistanceType.Poison, 75, 90);
            SetResistance(ResistanceType.Energy, 50, 65);

            SetSkill(SkillName.Anatomy, 75.1, 100.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 90.1, 100.0);
            SetSkill(SkillName.Meditation, 75.1, 100.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);
            SetSkill(SkillName.AnimalTaming, 90.1, 100.0);
            SetSkill(SkillName.AnimalLore, 90.1, 100.0);

            Fame = 7000;
            Karma = -7000;

            VirtualArmor = 60;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
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
                        case 0: this.Say(true, "My beasts will tear you apart!"); break;
                        case 1: this.Say(true, "Feel the wrath of my creatures!"); break;
                        case 2: this.Say(true, "You cannot escape my pets!"); break;
                        case 3: this.Say(true, "Attack, my loyal beasts!"); break;
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
            AddLoot(LootPack.FilthyRich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "Even in death, my beasts will avenge me..."); break;
                case 1: this.Say(true, "You may have won this time..."); break;
            }

            PackItem(new DragonBlood(Utility.RandomMinMax(10, 20)));
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
                        case 0: this.Say(true, "You'll regret that!"); break;
                        case 1: this.Say(true, "My beasts will make you pay!"); break;
                        case 2: this.Say(true, "That's not enough to stop me!"); break;
                        case 3: this.Say(true, "You can't defeat us!"); break;
                    }
                    
                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }
                
            return base.Damage(amount, from);
        }

        public Beastmaster(Serial serial) : base(serial)
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
