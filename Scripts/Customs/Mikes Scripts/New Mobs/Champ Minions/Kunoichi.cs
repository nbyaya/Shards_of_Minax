using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a kunoichi")]
    public class Kunoichi : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public Kunoichi() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            Body = 0x191;
            Name = NameList.RandomName("female");
            Title = "the Kunoichi";

            Item kimono = new FemaleKimono();
            kimono.Hue = Utility.RandomNeutralHue();
            AddItem(kimono);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            Item boots = new ThighBoots(Utility.RandomNeutralHue());
            AddItem(boots);

            Item weapon = new Dagger();
            AddItem(weapon);
            weapon.Movable = false;
            
            SetStr(400, 600);
            SetDex(200, 300);
            SetInt(100, 150);

            SetHits(300, 500);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 40, 55);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 80, 100);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.Anatomy, 60.1, 80.0);
            SetSkill(SkillName.Fencing, 90.1, 100.0);
            SetSkill(SkillName.MagicResist, 75.5, 90.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Ninjitsu, 80.1, 100.0);
            SetSkill(SkillName.Hiding, 80.1, 100.0);
            SetSkill(SkillName.Poisoning, 100.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 40;

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
                        case 0: this.Say(true, "You can't escape my blade!"); break;
                        case 1: this.Say(true, "I move like the shadows."); break;
                        case 2: this.Say(true, "Feel the sting of my poison!"); break;
                        case 3: this.Say(true, "Your end is near!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(200, 300);
            AddLoot(LootPack.Rich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "My mission... fails..."); break;
                case 1: this.Say(true, "You... got lucky..."); break;
            }

            PackItem(new Nightshade(Utility.RandomMinMax(10, 20)));
        }

        public Kunoichi(Serial serial) : base(serial)
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
