using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a ram rider")]
    public class RamRider : BaseCreature
    {
        private TimeSpan m_ChargeDelay = TimeSpan.FromSeconds(15.0); // time between charges
        public DateTime m_NextChargeTime;

        [Constructable]
        public RamRider() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 2;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Ram Rider";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Ram Rider";
            }

            Item armor = new PlateChest();
            AddItem(armor);
            
            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item pants = new PlateLegs();
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            Item weapon = new WarMace();
            AddItem(hair);
            AddItem(pants);
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

            SetStr(900, 1200);
            SetDex(150, 200);
            SetInt(50, 75);

            SetHits(800, 1000);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 70, 85);
            SetResistance(ResistanceType.Fire, 50, 65);
            SetResistance(ResistanceType.Cold, 40, 55);
            SetResistance(ResistanceType.Poison, 50, 65);
            SetResistance(ResistanceType.Energy, 40, 55);

            SetSkill(SkillName.Anatomy, 75.1, 100.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.MagicResist, 80.1, 100.0);
            SetSkill(SkillName.Wrestling, 80.1, 100.0);

            Fame = 7000;
            Karma = -7000;

            VirtualArmor = 60;

            m_NextChargeTime = DateTime.Now + m_ChargeDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextChargeTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    this.Say(true, "Charge!");
                    this.Emote("*charges forward*");
                    DoChargeAttack(combatant);

                    m_NextChargeTime = DateTime.Now + m_ChargeDelay;
                }

                base.OnThink();
            }
        }

        public void DoChargeAttack(Mobile target)
        {
            if (target != null && target.Alive && !target.IsDeadBondedPet)
            {
                Direction = GetDirectionTo(target);
                MovingEffect(target, 0x36D4, 10, 1, false, false, 0, 0);

                Timer.DelayCall(TimeSpan.FromSeconds(0.5), delegate
                {
                    if (target.Alive && !target.IsDeadBondedPet)
                    {
                        target.SendMessage("You are knocked back by the charge!");
                        target.Animate(21, 6, 1, true, false, 0);
                        target.MovingEffect(this, 0x36D4, 10, 1, false, false, 0, 0);
                        target.Move(GetDirectionTo(target));
                        target.Stam -= 20; // reduce stamina as part of the charge impact
                    }
                });
            }
        }

        public override void GenerateLoot()
        {
            PackGold(300, 400);
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, 2);
        }

        public RamRider(Serial serial) : base(serial)
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
