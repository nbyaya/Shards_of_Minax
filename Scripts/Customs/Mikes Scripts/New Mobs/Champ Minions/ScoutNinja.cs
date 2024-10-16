using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a scout ninja")]
    public class ScoutNinja : BaseCreature
    {
        private TimeSpan m_SmokeBombDelay = TimeSpan.FromSeconds(15.0); // time between smoke bomb usage
        public DateTime m_NextSmokeBombTime;

        [Constructable]
        public ScoutNinja() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Scout Ninja";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Scout Ninja";
            }

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item ninjaHood = new HoodedShroudOfShadows();
            Item ninjaBelt = new LeatherNinjaBelt();
            Item ninjaPants = new LeatherNinjaPants();
            Item ninjaBoots = new NinjaTabi();

            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(hair);
            AddItem(ninjaHood);
            AddItem(ninjaBelt);
            AddItem(ninjaPants);
            AddItem(ninjaBoots);

            SetStr(300, 400);
            SetDex(250, 350);
            SetInt(150, 200);

            SetHits(400, 600);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 60.0, 80.0);
            SetSkill(SkillName.EvalInt, 70.0, 90.0);
            SetSkill(SkillName.Hiding, 90.0, 100.0);
            SetSkill(SkillName.Ninjitsu, 90.0, 100.0);
            SetSkill(SkillName.MagicResist, 75.0, 95.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.Wrestling, 70.0, 90.0);

            Fame = 4000;
            Karma = -4000;

            VirtualArmor = 40;

            m_NextSmokeBombTime = DateTime.Now + m_SmokeBombDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextSmokeBombTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    this.Say(true, "Behold my vanishing act!");
                    Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 30, 5052);
                    Effects.PlaySound(this.Location, this.Map, 0x1FD);
                    this.Hidden = true;
                    this.Combatant = null;
                    
                    m_NextSmokeBombTime = DateTime.Now + m_SmokeBombDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(100, 150);
            AddLoot(LootPack.Average);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "My mission... is complete..."); break;
                case 1: this.Say(true, "You will... pay..."); break;
            }

            PackItem(new SmokeBomb(Utility.RandomMinMax(1, 5)));
        }

        public ScoutNinja(Serial serial) : base(serial)
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
