using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a harpist")]
    public class Harpist : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between harpist speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public Harpist() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 2;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Harpist";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Harpist";
            }

            Item robe = new Robe();
            robe.Hue = Utility.RandomNeutralHue();
            AddItem(robe);

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

            Item harp = new Harp();
            AddItem(harp);
            harp.Movable = false;

            SetStr(400, 600);
            SetDex(150, 250);
            SetInt(300, 500);

            SetHits(300, 500);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 40, 60);
            SetResistance(ResistanceType.Fire, 30, 50);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 40, 60);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.Musicianship, 100.0);
            SetSkill(SkillName.Provocation, 100.0);
            SetSkill(SkillName.Peacemaking, 100.0);
            SetSkill(SkillName.Discordance, 100.0);
            SetSkill(SkillName.MagicResist, 75.0, 100.0);
            SetSkill(SkillName.Magery, 75.0, 100.0);

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
                        case 0: this.Say(true, "Let the music soothe your soul..."); break;
                        case 1: this.Say(true, "Feel the power of my melody!"); break;
                        case 2: this.Say(true, "Sleep now, and dream of harps..."); break;
                        case 3: this.Say(true, "You cannot resist the harp's charm!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;

                    if (Utility.RandomDouble() < 0.3)
                    {
                        if (Utility.RandomBool())
                            Mesmerize(combatant);
                        else
                            PutToSleep(combatant);
                    }
                }

                base.OnThink();
            }
        }

        private void Mesmerize(Mobile target)
        {
            target.Freeze(TimeSpan.FromSeconds(5.0));
            target.SendMessage("You are mesmerized by the harpist's music!");
        }

        private void PutToSleep(Mobile target)
        {
            target.Paralyze(TimeSpan.FromSeconds(5.0));
            target.SendMessage("You fall asleep to the harpist's lullaby!");
        }

        public override void GenerateLoot()
        {
            PackGold(100, 150);
            AddLoot(LootPack.Rich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "My music... fades away..."); break;
                case 1: this.Say(true, "The harp... it is silent now..."); break;
            }

            PackItem(new Harp());
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
                        case 0: this.Say(true, "You can't silence my music!"); break;
                        case 1: this.Say(true, "The harp still sings!"); break;
                        case 2: this.Say(true, "My melody is unstoppable!"); break;
                        case 3: this.Say(true, "The music will never die!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }

            return base.Damage(amount, from);
        }

        public Harpist(Serial serial) : base(serial)
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
