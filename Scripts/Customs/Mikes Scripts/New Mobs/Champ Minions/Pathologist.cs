using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a pathologist")]
    public class Pathologist : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between pathologist speech
        public DateTime m_NextSpeechTime;
        private TimeSpan m_DiseaseFrequency = TimeSpan.FromSeconds(30.0); // time between disease inflictions
        public DateTime m_NextDiseaseTime;

        [Constructable]
        public Pathologist() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Pathologist";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Pathologist";
            }

            Item robe = new Robe();
            robe.Hue = Utility.RandomBlueHue();
            AddItem(robe);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item boots = new Boots();
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            Item weapon = new BoneHarvester();
            AddItem(hair);
            AddItem(boots);
            AddItem(weapon);
            weapon.Movable = false;

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(600, 800);
            SetDex(150, 200);
            SetInt(500, 700);

            SetHits(400, 600);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 80, 100);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 8000;
            Karma = -8000;

            VirtualArmor = 40;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
            m_NextDiseaseTime = DateTime.Now + m_DiseaseFrequency;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
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
                        case 0: this.Say(true, "Prepare to be weakened!"); break;
                        case 1: this.Say(true, "I will sap your strength!"); break;
                        case 2: this.Say(true, "Feel the disease take hold!"); break;
                        case 3: this.Say(true, "You cannot escape my plagues!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }

            if (DateTime.Now >= m_NextDiseaseTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    InflictDisease(combatant);
                    m_NextDiseaseTime = DateTime.Now + m_DiseaseFrequency;
                }
            }
        }

        public void InflictDisease(Mobile target)
        {
            if (target == null || target.Deleted || !target.Alive || target.Map != this.Map)
                return;

            target.SendMessage("You feel a terrible disease taking hold!");

            int duration = Utility.RandomMinMax(10, 20); // disease duration in seconds
            int statReduction = Utility.RandomMinMax(5, 15); // amount of stat reduction

            // Reduce stats
            target.AddStatMod(new StatMod(StatType.Str, "PathologistStr", -statReduction, TimeSpan.FromSeconds(duration)));
            target.AddStatMod(new StatMod(StatType.Dex, "PathologistDex", -statReduction, TimeSpan.FromSeconds(duration)));
            target.AddStatMod(new StatMod(StatType.Int, "PathologistInt", -statReduction, TimeSpan.FromSeconds(duration)));
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(200, 250);
            AddLoot(LootPack.FilthyRich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "My work here is done..."); break;
                case 1: this.Say(true, "The disease... it consumes me..."); break;
            }

            PackItem(new Nightshade(Utility.RandomMinMax(10, 20)));
        }

        public Pathologist(Serial serial) : base(serial)
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
