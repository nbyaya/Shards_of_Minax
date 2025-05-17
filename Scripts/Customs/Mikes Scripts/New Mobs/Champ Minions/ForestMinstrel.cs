using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a forest minstrel")]
    public class ForestMinstrel : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between minstrel speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public ForestMinstrel() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 1;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Forest Minstrel";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Forest Minstrel";
            }

            Item robe = new Robe(Utility.RandomNeutralHue());
            Item boots = new Sandals(Utility.RandomNeutralHue());
            Item harp = new Harp();

            AddItem(robe);
            AddItem(boots);
            AddItem(harp);
            harp.Movable = false;

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

            SetStr(300, 500);
            SetDex(150, 250);
            SetInt(300, 400);

            SetHits(250, 350);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 60);
            SetResistance(ResistanceType.Fire, 20, 40);
            SetResistance(ResistanceType.Cold, 20, 40);
            SetResistance(ResistanceType.Poison, 30, 50);
            SetResistance(ResistanceType.Energy, 30, 50);

            SetSkill(SkillName.Anatomy, 60.1, 80.0);
            SetSkill(SkillName.EvalInt, 80.1, 100.0);
            SetSkill(SkillName.Magery, 80.1, 100.0);
            SetSkill(SkillName.Meditation, 60.1, 80.0);
            SetSkill(SkillName.MagicResist, 70.1, 90.0);
            SetSkill(SkillName.Musicianship, 100.0);
            SetSkill(SkillName.Provocation, 90.0, 100.0);
            SetSkill(SkillName.Discordance, 90.0, 100.0);
            SetSkill(SkillName.Peacemaking, 90.0, 100.0);

            Fame = 5000;
            Karma = 5000;

            VirtualArmor = 40;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override bool CanRummageCorpses { get { return true; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "The forest will protect us!"); break;
                        case 1: this.Say(true, "Feel the soothing melody of the woods."); break;
                        case 2: this.Say(true, "Nature's harmony will prevail!"); break;
                        case 3: this.Say(true, "You shall not harm my friends!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }

            HealAllies();
            DebuffEnemies();
        }

        private void HealAllies()
        {
            IPooledEnumerable eable = this.GetMobilesInRange(5);
            foreach (Mobile m in eable)
            {
                if (m != this && m is BaseCreature && ((BaseCreature)m).Controlled && m.Alive && !m.IsDeadBondedPet)
                {
                    m.Heal(Utility.RandomMinMax(10, 20));
                    m.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);
                    m.PlaySound(0x1F2);
                }
            }
            eable.Free();
        }

        private void DebuffEnemies()
        {
            IPooledEnumerable eable = this.GetMobilesInRange(5);
            foreach (Mobile m in eable)
            {
                if (m != this && m is BaseCreature && !((BaseCreature)m).Controlled && m.Alive)
                {
                    m.SendMessage("You feel weakened by the forest minstrel's melody.");
                    m.FixedParticles(0x374A, 10, 15, 5038, EffectLayer.Head);
                    m.PlaySound(0x1E2);
                    m.Dex -= Utility.RandomMinMax(5, 10);
                    m.Int -= Utility.RandomMinMax(5, 10);
                }
            }
            eable.Free();
        }

        public override void GenerateLoot()
        {
            PackGold(100, 200);
            PackItem(new Bandage(Utility.RandomMinMax(1, 15)));
            PackItem(new Harp());
        }

        public ForestMinstrel(Serial serial) : base(serial)
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
