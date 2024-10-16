using Server.Mobiles;
using System.Collections.Generic;

namespace Server.FarmingSystem
{
    [CorpseName("a rat corpse")]
    public class CropRat : BaseCreature
    {
        [Constructable]
        public CropRat()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a rat";
            Body = 238;
            BaseSoundID = 0xCC;

            SetStr(9);
            SetDex(35);
            SetInt(5);

            SetHits(6);
            SetMana(0);

            SetDamage(1, 2);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 10);

            SetSkill(SkillName.MagicResist, 4.0);
            SetSkill(SkillName.Tactics, 4.0);
            SetSkill(SkillName.Wrestling, 4.0);

            Fame = 150;
            Karma = -150;

            Tamable = false;
        }

        public CropRat(Serial serial)
            : base(serial)
        {
        }

        public override int Meat => 1;
        public override FoodType FavoriteFood => FoodType.Meat | FoodType.Fish | FoodType.Eggs | FoodType.GrainsAndHay;
        public override void GenerateLoot()
        {
            AddLoot(LootPack.Poor);
        }

        public override void OnThink()
        {
            if (0.1 >= Utility.RandomDouble() && Combatant == null)//10% chance
            {
                List<BaseFarmingCrop> crops = new List<BaseFarmingCrop>();

                IPooledEnumerable<Item> items = GetItemsInRange(2);//Range 2

                foreach (Item item in items)
                {
                    if (item is BaseFarmingCrop)
                    {
                        crops.Add(item as BaseFarmingCrop);
                    }
                }


                if(crops.Count > 0)
                {
                    int indextoeat = Utility.Random(0, crops.Count);

                    this.SetDirection(GetDirectionTo(crops[indextoeat].Location));
                    this.PublicOverheadMessage(Network.MessageType.Regular, 0, true, "Nom Nom Nom");

                    this.TargetLocation = crops[indextoeat].Location;


                    ToupzyFarmingSystem.RemoveCrop(crops[indextoeat]);
                    crops[indextoeat].Delete();
                }

            }

            base.OnThink();
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
