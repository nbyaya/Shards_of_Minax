using System;
using Server.Items;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("corpse of a saboteur")]
    public class Saboteur : BaseCreature
    {
        private TimeSpan m_ExplosiveDelay = TimeSpan.FromSeconds(15.0); // time between planting explosives
        public DateTime m_NextExplosiveTime;
        private List<ExplosiveCharge> m_Charges = new List<ExplosiveCharge>();

        [Constructable]
        public Saboteur() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Saboteur";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Saboteur";
            }

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item pants = new LongPants(Utility.RandomNeutralHue());
            Item boots = new ThighBoots(Utility.RandomNeutralHue());
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            Item weapon = new Dagger();
            AddItem(hair);
            AddItem(pants);
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

            SetStr(500, 700);
            SetDex(200, 300);
            SetInt(100, 200);

            SetHits(400, 600);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 70);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 30, 50);
            SetResistance(ResistanceType.Poison, 20, 40);
            SetResistance(ResistanceType.Energy, 20, 40);

            SetSkill(SkillName.Anatomy, 60.1, 80.0);
            SetSkill(SkillName.MagicResist, 80.5, 100.0);
            SetSkill(SkillName.Tactics, 70.1, 90.0);
            SetSkill(SkillName.Wrestling, 60.1, 80.0);

            Fame = 4500;
            Karma = -4500;

            VirtualArmor = 50;

            m_NextExplosiveTime = DateTime.Now + m_ExplosiveDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextExplosiveTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    PlantExplosive(combatant);
                    m_NextExplosiveTime = DateTime.Now + m_ExplosiveDelay;
                }
            }

            base.OnThink();
        }

        private void PlantExplosive(Mobile target)
        {
            ExplosiveCharge charge = new ExplosiveCharge();
            charge.MoveToWorld(this.Location, this.Map);
            m_Charges.Add(charge);

            Timer.DelayCall(TimeSpan.FromSeconds(5.0), new TimerStateCallback(DetonateExplosive), charge);
            this.Say(true, "Planting explosive charge!");
        }

        private void DetonateExplosive(object state)
        {
            ExplosiveCharge charge = (ExplosiveCharge)state;

            if (charge != null && !charge.Deleted)
            {
                charge.Explode();
                m_Charges.Remove(charge);
            }
        }

        public Saboteur(Serial serial) : base(serial)
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

    public class ExplosiveCharge : Item
    {
        [Constructable]
        public ExplosiveCharge() : base(0x1B72)
        {
            Movable = false;
            Visible = false;
        }

        public void Explode()
        {
            Effects.SendLocationEffect(this.Location, this.Map, 0x36BD, 20, 10);
            Effects.PlaySound(this.Location, this.Map, 0x307);

            this.Delete();
        }

        public ExplosiveCharge(Serial serial) : base(serial)
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
