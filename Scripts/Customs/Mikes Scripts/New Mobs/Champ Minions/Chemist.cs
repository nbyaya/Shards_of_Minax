using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a chemist")]
    public class Chemist : BaseCreature
    {
        private TimeSpan m_BombDelay = TimeSpan.FromSeconds(10.0); // time between bomb throws
        public DateTime m_NextBombTime;

        [Constructable]
        public Chemist() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Chemist";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Chemist";
            }

            Item robe = new Robe();
            Item hat = new WizardsHat();
            robe.Hue = Utility.RandomNeutralHue();
            hat.Hue = Utility.RandomNeutralHue();
            AddItem(robe);
            AddItem(hat);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            SetStr(300, 500);
            SetDex(150, 250);
            SetInt(400, 600);

            SetHits(300, 500);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Magery, 90.1, 100.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Meditation, 75.1, 100.0);
            SetSkill(SkillName.MagicResist, 90.5, 100.0);
            SetSkill(SkillName.Tactics, 60.1, 80.0);
            SetSkill(SkillName.Wrestling, 50.1, 70.0);

            Fame = 4500;
            Karma = -4500;

            VirtualArmor = 40;

            m_NextBombTime = DateTime.Now + m_BombDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextBombTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 10))
                {
                    ThrowChemicalBomb(combatant);
                    m_NextBombTime = DateTime.Now + m_BombDelay;
                }

                base.OnThink();
            }
        }

        private void ThrowChemicalBomb(Mobile target)
        {
            this.Say(true, "Take this, a taste of my latest concoction!");

            IEntity from = new Entity(Serial.Zero, this.Location, this.Map);
            IEntity to = new Entity(Serial.Zero, target.Location, target.Map);

            Effects.SendMovingEffect(from, to, 0x36D4, 7, 0, false, false, 0x480, 0);
            Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerStateCallback(Explode_Callback), target);
        }

        private void Explode_Callback(object state)
        {
            Mobile target = state as Mobile;

            if (target != null && target.Alive && target.Map == this.Map && target.InRange(this, 10))
            {
                AOS.Damage(target, this, Utility.RandomMinMax(20, 40), 0, 0, 0, 100, 0);
                target.SendMessage("You are engulfed in a toxic cloud!");
                target.ApplyPoison(this, Poison.Lethal);
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(200, 250);
            AddLoot(LootPack.Rich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "My experiments... they have failed..."); break;
                case 1: this.Say(true, "You... you have bested my formulas..."); break;
            }

            PackItem(new Bottle(Utility.RandomMinMax(10, 20)));
        }

        public Chemist(Serial serial) : base(serial)
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
