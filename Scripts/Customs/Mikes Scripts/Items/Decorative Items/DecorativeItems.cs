using System;
using Server;

namespace Server.Items
{
    public class WeaponBottle : Item
    {
        [Constructable]
        public WeaponBottle() : base(0xAF2D)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Weapon Bottle";
        }

        public WeaponBottle(Serial serial) : base(serial) { }

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
	
    public class HildebrandtTapestry : Item
    {
        [Constructable]
        public HildebrandtTapestry() : base(0xAF1D)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Hildebrandt Tapestry";
        }

        public HildebrandtTapestry(Serial serial) : base(serial) { }

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

    public class HildebrandtShield : Item
    {
        [Constructable]
        public HildebrandtShield() : base(0xAF1B)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Hildebrandt Shield";
        }

        public HildebrandtShield(Serial serial) : base(serial) { }

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
	
    public class HildebrandtBunting : Item
    {
        [Constructable]
        public HildebrandtBunting() : base(0xAF19)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Hildebrandt Bunting";
        }

        public HildebrandtBunting(Serial serial) : base(serial) { }

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
	
    public class HildebrandtFlag  : Item
    {
        [Constructable]
        public HildebrandtFlag () : base(0xAF12)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Hildebrandt Flag ";
        }

        public HildebrandtFlag (Serial serial) : base(serial) { }

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
	
    public class Shears  : Item
    {
        [Constructable]
        public Shears () : base(0xAF0A)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Shears";
        }

        public Shears (Serial serial) : base(serial) { }

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
	
    public class LeatherStrapBelt  : Item
    {
        [Constructable]
        public LeatherStrapBelt () : base(0xAF05)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Leather Ninja Belt";
        }

        public LeatherStrapBelt (Serial serial) : base(serial) { }

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
	
    public class LampPostA  : Item
    {
        [Constructable]
        public LampPostA () : base(0xAE0F)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Lamp Post";
        }

        public LampPostA (Serial serial) : base(serial) { }

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
	
    public class LampPostB  : Item
    {
        [Constructable]
        public LampPostB () : base(0xAE0D)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Lamp Post";
        }

        public LampPostB (Serial serial) : base(serial) { }

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
	
    public class LampPostC  : Item
    {
        [Constructable]
        public LampPostC () : base(0xAE0B)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Lamp Post";
        }

        public LampPostC (Serial serial) : base(serial) { }

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
	
    public class ArtisanHolidayTree  : Item
    {
        [Constructable]
        public ArtisanHolidayTree () : base(0xADE4)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Artisan Holiday Tree";
        }

        public ArtisanHolidayTree (Serial serial) : base(serial) { }

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
	
    public class Satchel  : Item
    {
        [Constructable]
        public Satchel () : base(0xAD77)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Satchel";
        }

        public Satchel (Serial serial) : base(serial) { }

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
	
    public class WatermelonSliced  : Item
    {
        [Constructable]
        public WatermelonSliced () : base(0xAC79)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Watermelon Sliced";
        }

        public WatermelonSliced (Serial serial) : base(serial) { }

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
	
    public class WatermelonTray  : Item
    {
        [Constructable]
        public WatermelonTray () : base(0xAC78)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Watermelon Tray";
        }

        public WatermelonTray (Serial serial) : base(serial) { }

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
	
    public class WatermelonFruit  : Item
    {
        [Constructable]
        public WatermelonFruit () : base(0xAC76)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Watermelon";
        }

        public WatermelonFruit (Serial serial) : base(serial) { }

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
	
    public class SalvageMachine  : Item
    {
        [Constructable]
        public SalvageMachine () : base(0xAC5A)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Salvage Machine";
        }

        public SalvageMachine (Serial serial) : base(serial) { }

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
	
    public class LargeWeatheredBook  : Item
    {
        [Constructable]
        public LargeWeatheredBook () : base(0xA930)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Large Weathered Book";
        }

        public LargeWeatheredBook (Serial serial) : base(serial) { }

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
	
    public class BookTwentyfive  : Item
    {
        [Constructable]
        public BookTwentyfive () : base(0xA92D)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Book Twentyfive";
        }

        public BookTwentyfive (Serial serial) : base(serial) { }

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
	
    public class SkullBottle  : Item
    {
        [Constructable]
        public SkullBottle () : base(0xA92B)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Skull Bottle";
        }

        public SkullBottle (Serial serial) : base(serial) { }

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
	
    public class TwentyfiveShield  : Item
    {
        [Constructable]
        public TwentyfiveShield () : base(0xA92A)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Twentyfive Shield";
        }

        public TwentyfiveShield (Serial serial) : base(serial) { }

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
	
    public class VenusFlytrap   : Item
    {
        [Constructable]
        public VenusFlytrap  () : base(0xA91D)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Venus Flytrap";
        }

        public VenusFlytrap  (Serial serial) : base(serial) { }

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

    public class ZombieHand   : Item
    {
        [Constructable]
        public ZombieHand  () : base(0xA85F)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Zombie Hand";
        }

        public ZombieHand  (Serial serial) : base(serial) { }

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
	
    public class CandleStick   : Item
    {
        [Constructable]
        public CandleStick  () : base(0xA858)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Candle Stick";
        }

        public CandleStick  (Serial serial) : base(serial) { }

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

    public class CharcuterieBoard   : Item
    {
        [Constructable]
        public CharcuterieBoard  () : base(0xA857)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Charcuterie Board";
        }

        public CharcuterieBoard  (Serial serial) : base(serial) { }

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

    public class DaggerSign   : Item
    {
        [Constructable]
        public DaggerSign  () : base(0xA854)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "DaggerSign ";
        }

        public DaggerSign  (Serial serial) : base(serial) { }

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

    public class ChaliceOfPilfering   : Item
    {
        [Constructable]
        public ChaliceOfPilfering  () : base(0xA853)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Chalice Of Pilfering";
        }

        public ChaliceOfPilfering  (Serial serial) : base(serial) { }

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

    public class LargeVat   : Item
    {
        [Constructable]
        public LargeVat  () : base(0xA84B)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Large Vat ";
        }

        public LargeVat  (Serial serial) : base(serial) { }

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

    public class CartographyTable   : Item
    {
        [Constructable]
        public CartographyTable  () : base(0xA849)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Cartography Table";
        }

        public CartographyTable  (Serial serial) : base(serial) { }

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

    public class CartographyDesk   : Item
    {
        [Constructable]
        public CartographyDesk  () : base(0xA833)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Cartography Desk";
        }

        public CartographyDesk  (Serial serial) : base(serial) { }

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

    public class HolidayCandleArran   : Item
    {
        [Constructable]
        public HolidayCandleArran  () : base(0xA7C6)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Holiday Candle";
        }

        public HolidayCandleArran  (Serial serial) : base(serial) { }

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

    public class CrabBushel   : Item
    {
        [Constructable]
        public CrabBushel  () : base(0xA7AE)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Crab Bushel";
        }

        public CrabBushel  (Serial serial) : base(serial) { }

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

    public class FishBasket   : Item
    {
        [Constructable]
        public FishBasket  () : base(0xA7AC)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Fish Basket";
        }

        public FishBasket  (Serial serial) : base(serial) { }

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

    public class KrakenTrophy   : Item
    {
        [Constructable]
        public KrakenTrophy  () : base(0xA7A9)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Kraken Trophy";
        }

        public KrakenTrophy  (Serial serial) : base(serial) { }

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

    public class DemonPlatter   : Item
    {
        [Constructable]
        public DemonPlatter  () : base(0xA7A7)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Demon Platter";
        }

        public DemonPlatter  (Serial serial) : base(serial) { }

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

    public class MedusaHead   : Item
    {
        [Constructable]
        public MedusaHead  () : base(0xA795)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Medusa Head";
        }

        public MedusaHead  (Serial serial) : base(serial) { }

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

    public class OpalBranch   : Item
    {
        [Constructable]
        public OpalBranch  () : base(0xA777)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Opal Branch";
        }

        public OpalBranch  (Serial serial) : base(serial) { }

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

    public class OrnateHarp   : Item
    {
        [Constructable]
        public OrnateHarp  () : base(0xA775)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Ornate Harp";
        }

        public OrnateHarp  (Serial serial) : base(serial) { }

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

    public class InfinitySymbol   : Item
    {
        [Constructable]
        public InfinitySymbol  () : base(0xA774)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Infinity Symbol";
        }

        public InfinitySymbol  (Serial serial) : base(serial) { }

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

    public class MysteryOrb   : Item
    {
        [Constructable]
        public MysteryOrb  () : base(0xA772)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Mystery Orb";
        }

        public MysteryOrb  (Serial serial) : base(serial) { }

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

    public class PowerGem   : Item
    {
        [Constructable]
        public PowerGem  () : base(0xA764)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Power Gem";
        }

        public PowerGem  (Serial serial) : base(serial) { }

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

    public class BrokenBottle   : Item
    {
        [Constructable]
        public BrokenBottle  () : base(0xA763)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Broken Bottle";
        }

        public BrokenBottle  (Serial serial) : base(serial) { }

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


    public class LargeTome   : Item
    {
        [Constructable]
        public LargeTome  () : base(0xA75F)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Large Tome";
        }

        public LargeTome  (Serial serial) : base(serial) { }

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

    public class SkullsStack   : Item
    {
        [Constructable]
        public SkullsStack  () : base(0xA74F)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Skulls Stack";
        }

        public SkullsStack  (Serial serial) : base(serial) { }

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

    public class GargoyleLamp   : Item
    {
        [Constructable]
        public GargoyleLamp  () : base(0xA748)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Gargoyle Lamp";
        }

        public GargoyleLamp  (Serial serial) : base(serial) { }

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

    public class ToolBox   : Item
    {
        [Constructable]
        public ToolBox  () : base(0xA744)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Tool Box";
        }

        public ToolBox  (Serial serial) : base(serial) { }

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

    public class EnchantedAnnealer   : Item
    {
        [Constructable]
        public EnchantedAnnealer  () : base(0xA73F)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Enchanted Annealer";
        }

        public EnchantedAnnealer  (Serial serial) : base(serial) { }

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

    public class YardCupid   : Item
    {
        [Constructable]
        public YardCupid  () : base(0xA734)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Yard Cupid";
        }

        public YardCupid  (Serial serial) : base(serial) { }

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

    public class GargishTotem   : Item
    {
        [Constructable]
        public GargishTotem  () : base(0xA725)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Gargish Totem";
        }

        public GargishTotem  (Serial serial) : base(serial) { }

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

    public class MiniYew   : Item
    {
        [Constructable]
        public MiniYew  () : base(0xA724)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Mini Yew";
        }

        public MiniYew  (Serial serial) : base(serial) { }

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

    public class StrappedBooks   : Item
    {
        [Constructable]
        public StrappedBooks  () : base(0xA721)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Strapped Books";
        }

        public StrappedBooks  (Serial serial) : base(serial) { }

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

    public class FishingBear   : Item
    {
        [Constructable]
        public FishingBear  () : base(0xA64C)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Fishing Bear";
        }

        public FishingBear  (Serial serial) : base(serial) { }

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

    public class SkullShield   : Item
    {
        [Constructable]
        public SkullShield  () : base(0xA64A)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Skull Shield";
        }

        public SkullShield  (Serial serial) : base(serial) { }

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

    public class BrassFountain   : Item
    {
        [Constructable]
        public BrassFountain  () : base(0xA633)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Brass Fountain";
        }

        public BrassFountain  (Serial serial) : base(serial) { }

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

    public class FountainWall   : Item
    {
        [Constructable]
        public FountainWall  () : base(0xA62D)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Fountain Wall";
        }

        public FountainWall  (Serial serial) : base(serial) { }

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

    public class WaterFountain   : Item
    {
        [Constructable]
        public WaterFountain  () : base(0xA62A)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Water Fountain";
        }

        public WaterFountain  (Serial serial) : base(serial) { }

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

    public class SpookyGhost   : Item
    {
        [Constructable]
        public SpookyGhost  () : base(0xA623)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "SpookyGhost";
        }

        public SpookyGhost  (Serial serial) : base(serial) { }

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
	
    public class Birdhouse   : Item
    {
        [Constructable]
        public Birdhouse  () : base(0xA608)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Birdhouse";
        }

        public Birdhouse  (Serial serial) : base(serial) { }

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
	
    public class AlchemistBookcase   : Item
    {
        [Constructable]
        public AlchemistBookcase  () : base(0xA5E8)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Alchemist Bookcase";
        }

        public AlchemistBookcase  (Serial serial) : base(serial) { }

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
	
    public class MagicOrb   : Item
    {
        [Constructable]
        public MagicOrb  () : base(0xA5DB)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "MagicOrb";
        }

        public MagicOrb  (Serial serial) : base(serial) { }

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
	
    public class ExcitingTome   : Item
    {
        [Constructable]
        public ExcitingTome  () : base(0xA5D1)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Exciting Tome";
        }

        public ExcitingTome  (Serial serial) : base(serial) { }

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
	
    public class ForbiddenTome   : Item
    {
        [Constructable]
        public ForbiddenTome  () : base(0xA5D0)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "ForbiddenTome";
        }

        public ForbiddenTome  (Serial serial) : base(serial) { }

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
	
    public class ColoredLamppost   : Item
    {
        [Constructable]
        public ColoredLamppost  () : base(0xA5BB)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Colored Lamppost";
        }

        public ColoredLamppost  (Serial serial) : base(serial) { }

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
	
    public class WitchesCauldron   : Item
    {
        [Constructable]
        public WitchesCauldron  () : base(0xA5B4)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Witches Cauldron";
        }

        public WitchesCauldron  (Serial serial) : base(serial) { }

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
	
    public class MagicBookStand    : Item
    {
        [Constructable]
        public MagicBookStand   () : base(0xA588)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Magic Book Stand";
        }

        public MagicBookStand   (Serial serial) : base(serial) { }

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
	
    public class RootThrone    : Item
    {
        [Constructable]
        public RootThrone   () : base(0xA586)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Root Throne";
        }

        public RootThrone   (Serial serial) : base(serial) { }

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
	
    public class ImportantBooks    : Item
    {
        [Constructable]
        public ImportantBooks   () : base(0xA585)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Important Books";
        }

        public ImportantBooks   (Serial serial) : base(serial) { }

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
	
    public class EssentialBooks    : Item
    {
        [Constructable]
        public EssentialBooks   () : base(0xA584)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Essential Books";
        }

        public EssentialBooks   (Serial serial) : base(serial) { }

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

    public class RookStone    : Item
    {
        [Constructable]
        public RookStone   () : base(0xA583)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Rook Stone";
        }

        public RookStone   (Serial serial) : base(serial) { }

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

    public class KnightStone    : Item
    {
        [Constructable]
        public KnightStone   () : base(0xA582)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Knight Stone";
        }

        public KnightStone   (Serial serial) : base(serial) { }

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

    public class TinyWizard    : Item
    {
        [Constructable]
        public TinyWizard   () : base(0xA572)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "TinyWizard";
        }

        public TinyWizard   (Serial serial) : base(serial) { }

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

    public class QuestWineRack    : Item
    {
        [Constructable]
        public QuestWineRack   () : base(0xA56A)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Wine Rack";
        }

        public QuestWineRack   (Serial serial) : base(serial) { }

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

    public class BirdStatue    : Item
    {
        [Constructable]
        public BirdStatue   () : base(0xA566)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Bird Statue";
        }

        public BirdStatue   (Serial serial) : base(serial) { }

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

    public class GlassFurnace    : Item
    {
        [Constructable]
        public GlassFurnace   () : base(0xA531)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Glass Furnace";
        }

        public GlassFurnace   (Serial serial) : base(serial) { }

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

    public class GlassTable    : Item
    {
        [Constructable]
        public GlassTable   () : base(0xA52E)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Glass Table";
        }

        public GlassTable   (Serial serial) : base(serial) { }

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

    public class ValentineTeddybear    : Item
    {
        [Constructable]
        public ValentineTeddybear   () : base(0xA516)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Valentine Teddybear";
        }

        public ValentineTeddybear   (Serial serial) : base(serial) { }

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

    public class LayingChicken    : Item
    {
        [Constructable]
        public LayingChicken   () : base(0xA514)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Laying Chicken";
        }

        public LayingChicken   (Serial serial) : base(serial) { }

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

    public class HorseToken    : Item
    {
        [Constructable]
        public HorseToken   () : base(0xA511)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "A Compact Horse";
        }

        public HorseToken   (Serial serial) : base(serial) { }

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

    public class CowToken    : Item
    {
        [Constructable]
        public CowToken   () : base(0xA50F)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Compact Cow";
        }

        public CowToken   (Serial serial) : base(serial) { }

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

    public class SkullRing    : Item
    {
        [Constructable]
        public SkullRing   () : base(0xA4F2)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "SkullRing";
        }

        public SkullRing   (Serial serial) : base(serial) { }

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

    public class HangingMask    : Item
    {
        [Constructable]
        public HangingMask   () : base(0xA4F0)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "HangingMask";
        }

        public HangingMask   (Serial serial) : base(serial) { }

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

    public class PunishmentStocks    : Item
    {
        [Constructable]
        public PunishmentStocks   () : base(0xA4ED)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Punishment Stocks";
        }

        public PunishmentStocks   (Serial serial) : base(serial) { }

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

    public class JudasCradle    : Item
    {
        [Constructable]
        public JudasCradle   () : base(0xA4EC)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Judas Cradle";
        }

        public JudasCradle   (Serial serial) : base(serial) { }

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

    public class SpikedChair    : Item
    {
        [Constructable]
        public SpikedChair   () : base(0xA4EB)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "SpikedChair";
        }

        public SpikedChair   (Serial serial) : base(serial) { }

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

    public class MeatHooks    : Item
    {
        [Constructable]
        public MeatHooks   () : base(0xA4E8)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Meat Hooks";
        }

        public MeatHooks   (Serial serial) : base(serial) { }

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

    public class ExoticWhistle    : Item
    {
        [Constructable]
        public ExoticWhistle   () : base(0xA4E7)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Exotic Whistle";
        }

        public ExoticWhistle   (Serial serial) : base(serial) { }

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

    public class CowPoo    : Item
    {
        [Constructable]
        public CowPoo   () : base(0xA4E5)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Cow Poo";
        }

        public CowPoo   (Serial serial) : base(serial) { }

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

    public class TinTub    : Item
    {
        [Constructable]
        public TinTub   () : base(0xA4E2)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "TinTub";
        }

        public TinTub   (Serial serial) : base(serial) { }

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

    public class GalvanizedTub    : Item
    {
        [Constructable]
        public GalvanizedTub   () : base(0xA4DF)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "GalvanizedTub";
        }

        public GalvanizedTub   (Serial serial) : base(serial) { }

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

    public class FancyXmasTree    : Item
    {
        [Constructable]
        public FancyXmasTree   () : base(0xA4D0)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Fancy Xmas Tree";
        }

        public FancyXmasTree   (Serial serial) : base(serial) { }

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

    public class SeaSerpantSteak    : Item
    {
        [Constructable]
        public SeaSerpantSteak   () : base(0xA423)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "SeaSerpantSteak";
        }

        public SeaSerpantSteak   (Serial serial) : base(serial) { }

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

    public class FunPumpkinCannon    : Item
    {
        [Constructable]
        public FunPumpkinCannon   () : base(0xA3FE)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Pumpkin Cannon";
        }

        public FunPumpkinCannon   (Serial serial) : base(serial) { }

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

    public class HorrorPumpkin    : Item
    {
        [Constructable]
        public HorrorPumpkin   () : base(0xA403)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "HorrorPumpkin";
        }

        public HorrorPumpkin   (Serial serial) : base(serial) { }

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

    public class PetRock    : Item
    {
        [Constructable]
        public PetRock   () : base(0xA3E9)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Pet Rock";
        }

        public PetRock   (Serial serial) : base(serial) { }

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

    public class BlueSand    : Item
    {
        [Constructable]
        public BlueSand   () : base(0xA3E8)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Blue Sand";
        }

        public BlueSand   (Serial serial) : base(serial) { }

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

    public class MemorialCopper    : Item
    {
        [Constructable]
        public MemorialCopper   () : base(0xA3E7)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Memorial Copper";
        }

        public MemorialCopper   (Serial serial) : base(serial) { }

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

    public class FancyCopperWings    : Item
    {
        [Constructable]
        public FancyCopperWings   () : base(0xA3DE)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Fancy Copper Wings";
        }

        public FancyCopperWings   (Serial serial) : base(serial) { }

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

    public class DecorativeFishTank    : Item
    {
        [Constructable]
        public DecorativeFishTank   () : base(0xA3A6)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Decorative FishTank";
        }

        public DecorativeFishTank   (Serial serial) : base(serial) { }

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

    public class BabyLavos    : Item
    {
        [Constructable]
        public BabyLavos   () : base(0xA3A4)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "BabyLavos";
        }

        public BabyLavos   (Serial serial) : base(serial) { }

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

    public class MutantStarfish    : Item
    {
        [Constructable]
        public MutantStarfish   () : base(0xA395)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Mutant Starfish";
        }

        public MutantStarfish   (Serial serial) : base(serial) { }

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

    public class FancyCopperSunflower    : Item
    {
        [Constructable]
        public FancyCopperSunflower   () : base(0xA35D)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "FancyCopperSunflower";
        }

        public FancyCopperSunflower   (Serial serial) : base(serial) { }

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

    public class MarbleHourglass    : Item
    {
        [Constructable]
        public MarbleHourglass   () : base(0xA33A)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "MarbleHourglass";
        }

        public MarbleHourglass   (Serial serial) : base(serial) { }

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

    public class PileOfChains    : Item
    {
        [Constructable]
        public PileOfChains   () : base(0xA32E)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Pile Of Chains";
        }

        public PileOfChains   (Serial serial) : base(serial) { }

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

    public class HeavyAnchor    : Item
    {
        [Constructable]
        public HeavyAnchor   () : base(0xA32B)
        {
            Weight = 10.0;
            Hue = 0;
            Name = "Heavy Anchor";
        }

        public HeavyAnchor   (Serial serial) : base(serial) { }

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

    public class MilkingPail    : Item
    {
        [Constructable]
        public MilkingPail   () : base(0xA324)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Milking Pail";
        }

        public MilkingPail   (Serial serial) : base(serial) { }

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

    public class ExoticShipInABottle    : Item
    {
        [Constructable]
        public ExoticShipInABottle   () : base(0xA321)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Ship In A Bottle";
        }

        public ExoticShipInABottle   (Serial serial) : base(serial) { }

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

    public class FieldPlow    : Item
    {
        [Constructable]
        public FieldPlow   () : base(0xA320)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "FieldPlow";
        }

        public FieldPlow   (Serial serial) : base(serial) { }

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

    public class WaterWell    : Item
    {
        [Constructable]
        public WaterWell   () : base(0xA300)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Portable Well";
        }

        public WaterWell   (Serial serial) : base(serial) { }

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

    public class FeedingTrough    : Item
    {
        [Constructable]
        public FeedingTrough   () : base(0xA2FE)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "FeedingTrough";
        }

        public FeedingTrough   (Serial serial) : base(serial) { }

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

    public class ButterChurn    : Item
    {
        [Constructable]
        public ButterChurn   () : base(0xA2FD)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "ButterChurn";
        }

        public ButterChurn   (Serial serial) : base(serial) { }

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

    public class BonFire    : Item
    {
        [Constructable]
        public BonFire   () : base(0xA2F7)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "BonFire";
        }

        public BonFire   (Serial serial) : base(serial) { }

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

    public class NixieStatue    : Item
    {
        [Constructable]
        public NixieStatue   () : base(0xA2C7)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "NixieStatue";
        }

        public NixieStatue   (Serial serial) : base(serial) { }

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

    public class SmugglersCrate    : Item
    {
        [Constructable]
        public SmugglersCrate   () : base(0xA2C5)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "SmugglersCrate";
        }

        public SmugglersCrate   (Serial serial) : base(serial) { }

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

    public class PersonalCannon    : Item
    {
        [Constructable]
        public PersonalCannon   () : base(0xA2C3)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Personal Cannon";
        }

        public PersonalCannon   (Serial serial) : base(serial) { }

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

    public class TrophieAward    : Item
    {
        [Constructable]
        public TrophieAward   () : base(0xA299)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Trophie Award";
        }

        public TrophieAward   (Serial serial) : base(serial) { }

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

    public class RibbonAward    : Item
    {
        [Constructable]
        public RibbonAward   () : base(0xA297)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Ribbon Award";
        }

        public RibbonAward   (Serial serial) : base(serial) { }

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

    public class GamerGirlFeet    : Item
    {
        [Constructable]
        public GamerGirlFeet   () : base(0xA296)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Gamer Girl Feet";
        }

        public GamerGirlFeet   (Serial serial) : base(serial) { }

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

    public class ExoticBoots    : Item
    {
        [Constructable]
        public ExoticBoots   () : base(0xA28D)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Exotic Boots";
        }

        public ExoticBoots   (Serial serial) : base(serial) { }

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

    public class LovelyLilies    : Item
    {
        [Constructable]
        public LovelyLilies   () : base(0xA286)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Lovely Lilies";
        }

        public LovelyLilies   (Serial serial) : base(serial) { }

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

    public class TotumPole    : Item
    {
        [Constructable]
        public TotumPole   () : base(0xA276)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "TotumP ole";
        }

        public TotumPole   (Serial serial) : base(serial) { }

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

    public class ExoticWoods    : Item
    {
        [Constructable]
        public ExoticWoods   () : base(0xA275)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Exotic Woods";
        }

        public ExoticWoods   (Serial serial) : base(serial) { }

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

    public class RareMinerals    : Item
    {
        [Constructable]
        public RareMinerals   () : base(0xA273)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Rare Minerals";
        }

        public RareMinerals   (Serial serial) : base(serial) { }

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
	
    public class OldEmbroideryTool    : Item
    {
        [Constructable]
        public OldEmbroideryTool   () : base(0xA20A)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Old Embroidery Tool";
        }

        public OldEmbroideryTool   (Serial serial) : base(serial) { }

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
	
    public class SpiderTree    : Item
    {
        [Constructable]
        public SpiderTree   () : base(0xA1F0)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Spider Tree";
        }

        public SpiderTree   (Serial serial) : base(serial) { }

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
	
    public class StarMap    : Item
    {
        [Constructable]
        public StarMap   () : base(0xA1E4)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "StarMap";
        }

        public StarMap   (Serial serial) : base(serial) { }

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
	
    public class EarthRelic    : Item
    {
        [Constructable]
        public EarthRelic   () : base(0xA1DD)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "EarthRelic";
        }

        public EarthRelic   (Serial serial) : base(serial) { }

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
	
    public class WindRelic    : Item
    {
        [Constructable]
        public WindRelic   () : base(0xA1DC)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "WindRelic";
        }

        public WindRelic   (Serial serial) : base(serial) { }

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
	
    public class FireRelic    : Item
    {
        [Constructable]
        public FireRelic   () : base(0xA1DB)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "FireRelic";
        }

        public FireRelic   (Serial serial) : base(serial) { }

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
	
    public class WaterRelic    : Item
    {
        [Constructable]
        public WaterRelic   () : base(0xA1DA)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "WaterRelic";
        }

        public WaterRelic   (Serial serial) : base(serial) { }

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
	
    public class ImperiumCrest    : Item
    {
        [Constructable]
        public ImperiumCrest   () : base(0xA1C9)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Imperium Crest";
        }

        public ImperiumCrest   (Serial serial) : base(serial) { }

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
	
    public class TribalHelm    : Item
    {
        [Constructable]
        public TribalHelm   () : base(0xA1C7)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Tribal Helm";
        }

        public TribalHelm   (Serial serial) : base(serial) { }

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
	
    public class AstroLabe    : Item
    {
        [Constructable]
        public AstroLabe   () : base(0xA17B)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "AstroLabe";
        }

        public AstroLabe   (Serial serial) : base(serial) { }

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
	
    public class TrexSkull    : Item
    {
        [Constructable]
        public TrexSkull   () : base(0xA179)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Trex Skull";
        }

        public TrexSkull   (Serial serial) : base(serial) { }

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

    public class SabertoothSkull    : Item
    {
        [Constructable]
        public SabertoothSkull   () : base(0xA177)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Sabertooth Skull";
        }

        public SabertoothSkull   (Serial serial) : base(serial) { }

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

    public class AmatureTelescope    : Item
    {
        [Constructable]
        public AmatureTelescope   () : base(0xA12C)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Amature Telescope";
        }

        public AmatureTelescope   (Serial serial) : base(serial) { }

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

    public class PicnicDayBasket    : Item
    {
        [Constructable]
        public PicnicDayBasket   () : base(0xA0DC)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Picnic Basket";
        }

        public PicnicDayBasket   (Serial serial) : base(serial) { }

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

    public class Hamburgers    : Item
    {
        [Constructable]
        public Hamburgers   () : base(0xA0DA)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Hamburgers";
        }

        public Hamburgers   (Serial serial) : base(serial) { }

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

    public class Hotdogs    : Item
    {
        [Constructable]
        public Hotdogs   () : base(0xA0D8)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Hotdogs";
        }

        public Hotdogs   (Serial serial) : base(serial) { }

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

    public class RareSausage    : Item
    {
        [Constructable]
        public RareSausage   () : base(0xA0D7)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Rare Sausage";
        }

        public RareSausage   (Serial serial) : base(serial) { }

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

    public class HeartPillow    : Item
    {
        [Constructable]
        public HeartPillow   () : base(0xA0A1)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Heart Pillow";
        }

        public HeartPillow   (Serial serial) : base(serial) { }

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

    public class FrostToken    : Item
    {
        [Constructable]
        public FrostToken   () : base(0xA094)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Frost Token";
        }

        public FrostToken   (Serial serial) : base(serial) { }

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

    public class HeartChair    : Item
    {
        [Constructable]
        public HeartChair   () : base(0xA05D)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "HeartChair";
        }

        public HeartChair   (Serial serial) : base(serial) { }

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

    public class DailyNewspaper    : Item
    {
        [Constructable]
        public DailyNewspaper   () : base(0x9FB4)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Daily Newspaper";
        }

        public DailyNewspaper   (Serial serial) : base(serial) { }

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

    public class WeddingChest    : Item
    {
        [Constructable]
        public WeddingChest   () : base(0x9F95)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Wedding Chest";
        }

        public WeddingChest   (Serial serial) : base(serial) { }

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

    public class PersonalMortor    : Item
    {
        [Constructable]
        public PersonalMortor   () : base(0x9F93)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "PersonalMortor";
        }

        public PersonalMortor   (Serial serial) : base(serial) { }

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

    public class GarbageBag    : Item
    {
        [Constructable]
        public GarbageBag   () : base(0x9F85)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Garbage Bag";
        }

        public GarbageBag   (Serial serial) : base(serial) { }

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

    public class HorrodrickCube    : Item
    {
        [Constructable]
        public HorrodrickCube   () : base(0x9F6C)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Horrodrick Cube";
        }

        public HorrodrickCube   (Serial serial) : base(serial) { }

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

    public class HotFlamingScarecrow    : Item
    {
        [Constructable]
        public HotFlamingScarecrow   () : base(0x9F34)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Flaming Scarecrow";
        }

        public HotFlamingScarecrow   (Serial serial) : base(serial) { }

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

    public class OldBones    : Item
    {
        [Constructable]
        public OldBones   () : base(0x9F1E)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Old Bones";
        }

        public OldBones   (Serial serial) : base(serial) { }

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

    public class WeddingCandelabra    : Item
    {
        [Constructable]
        public WeddingCandelabra   () : base(0x9EF1)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Wedding Candelabra";
        }

        public WeddingCandelabra   (Serial serial) : base(serial) { }

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

    public class SkullIncense    : Item
    {
        [Constructable]
        public SkullIncense   () : base(0x9EEE)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Skull Incense";
        }

        public SkullIncense   (Serial serial) : base(serial) { }

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

    public class SatanicTable    : Item
    {
        [Constructable]
        public SatanicTable   () : base(0x9EE5)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Satanic Table";
        }

        public SatanicTable   (Serial serial) : base(serial) { }

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

    public class ChocolateFountain    : Item
    {
        [Constructable]
        public ChocolateFountain   () : base(0x9EBF)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Chocolate Fountain";
        }

        public ChocolateFountain   (Serial serial) : base(serial) { }

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

    public class BrandingIron    : Item
    {
        [Constructable]
        public BrandingIron   () : base(0x9E88)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Branding Iron";
        }

        public BrandingIron   (Serial serial) : base(serial) { }

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

    public class ExoticPlum    : Item
    {
        [Constructable]
        public ExoticPlum   () : base(0x9E86)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Exotic Plum";
        }

        public ExoticPlum   (Serial serial) : base(serial) { }

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

    public class GraniteHammer    : Item
    {
        [Constructable]
        public GraniteHammer   () : base(0x9E7E)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Granite Hammer";
        }

        public GraniteHammer   (Serial serial) : base(serial) { }

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

    public class MiniKeg    : Item
    {
        [Constructable]
        public MiniKeg   () : base(0x9E36)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Mini Keg";
        }

        public MiniKeg   (Serial serial) : base(serial) { }

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

    public class CarpentryTalisman    : Item
    {
        [Constructable]
        public CarpentryTalisman   () : base(0x9E2C)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Carpentry Talisman";
        }

        public CarpentryTalisman   (Serial serial) : base(serial) { }

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

    public class TinkeringTalisman    : Item
    {
        [Constructable]
        public TinkeringTalisman   () : base(0x9E2B)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Tinkering Talisman";
        }

        public TinkeringTalisman   (Serial serial) : base(serial) { }

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

    public class BlacksmithTalisman    : Item
    {
        [Constructable]
        public BlacksmithTalisman   () : base(0x9E2A)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Blacksmith Talisman";
        }

        public BlacksmithTalisman   (Serial serial) : base(serial) { }

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

    public class FletchingTalisman    : Item
    {
        [Constructable]
        public FletchingTalisman   () : base(0x9E29)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Fletching Talisman";
        }

        public FletchingTalisman   (Serial serial) : base(serial) { }

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

    public class InscriptionTalisman    : Item
    {
        [Constructable]
        public InscriptionTalisman   () : base(0x9E28)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Inscription Talisman";
        }

        public InscriptionTalisman   (Serial serial) : base(serial) { }

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

    public class CookingTalisman    : Item
    {
        [Constructable]
        public CookingTalisman   () : base(0x9E27)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Cooking Talisman";
        }

        public CookingTalisman   (Serial serial) : base(serial) { }

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

    public class AlchemyTalisman    : Item
    {
        [Constructable]
        public AlchemyTalisman   () : base(0x9E26)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Alchemy Talisman";
        }

        public AlchemyTalisman   (Serial serial) : base(serial) { }

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

    public class TailoringTalisman    : Item
    {
        [Constructable]
        public TailoringTalisman   () : base(0x9E25)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Tailoring Talisman";
        }

        public TailoringTalisman   (Serial serial) : base(serial) { }

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

    public class MasterGyro    : Item
    {
        [Constructable]
        public MasterGyro   () : base(0x9CEF)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Master Gyro";
        }

        public MasterGyro   (Serial serial) : base(serial) { }

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

    public class MultiPump    : Item
    {
        [Constructable]
        public MultiPump   () : base(0x9CE9)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "MultiPump";
        }

        public MultiPump   (Serial serial) : base(serial) { }

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

    public class ZttyCrystal    : Item
    {
        [Constructable]
        public ZttyCrystal   () : base(0x9CB5)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Ztty Crystal";
        }

        public ZttyCrystal   (Serial serial) : base(serial) { }

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

    public class SoftTowel    : Item
    {
        [Constructable]
        public SoftTowel   () : base(0x9C48)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Soft Towel";
        }

        public SoftTowel   (Serial serial) : base(serial) { }

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

    public class CarvingMachine    : Item
    {
        [Constructable]
        public CarvingMachine   () : base(0x9C25)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Carving Machine";
        }

        public CarvingMachine   (Serial serial) : base(serial) { }

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

    public class LexxVase    : Item
    {
        [Constructable]
        public LexxVase   () : base(0x9BCA)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Lexx Vase";
        }

        public LexxVase   (Serial serial) : base(serial) { }

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

    public class ZebulinVase    : Item
    {
        [Constructable]
        public ZebulinVase   () : base(0x9BC7)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Zebulin Vase";
        }

        public ZebulinVase   (Serial serial) : base(serial) { }

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

    public class HandOfFate    : Item
    {
        [Constructable]
        public HandOfFate   () : base(0x9BB5)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Hand Of Fate";
        }

        public HandOfFate   (Serial serial) : base(serial) { }

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

    public class HutFlower    : Item
    {
        [Constructable]
        public HutFlower   () : base(0x9B11)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Hut Flower";
        }

        public HutFlower   (Serial serial) : base(serial) { }

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

    public class DecorativeOrchid    : Item
    {
        [Constructable]
        public DecorativeOrchid   () : base(0x9B10)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Decorative Orchid";
        }

        public DecorativeOrchid   (Serial serial) : base(serial) { }

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

    public class MiniCherryTree    : Item
    {
        [Constructable]
        public MiniCherryTree   () : base(0x9B0F)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Mini Cherry Tree";
        }

        public MiniCherryTree   (Serial serial) : base(serial) { }

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

    public class FestiveChampagne    : Item
    {
        [Constructable]
        public FestiveChampagne   () : base(0x9AAD)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Festive Champagne";
        }

        public FestiveChampagne   (Serial serial) : base(serial) { }

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

    public class GlassOfBubbly    : Item
    {
        [Constructable]
        public GlassOfBubbly   () : base(0x9AAA)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Glass Of Bubbly";
        }

        public GlassOfBubbly   (Serial serial) : base(serial) { }

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

    public class AutoPounder    : Item
    {
        [Constructable]
        public AutoPounder   () : base(0x9A89)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "AutoPounder";
        }

        public AutoPounder   (Serial serial) : base(serial) { }

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

    public class FancySewingMachine    : Item
    {
        [Constructable]
        public FancySewingMachine   () : base(0x9A40)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Sewing Machine";
        }

        public FancySewingMachine   (Serial serial) : base(serial) { }

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

    public class FancyCrystalSkull    : Item
    {
        [Constructable]
        public FancyCrystalSkull   () : base(0x9A1B)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Crystal Skull";
        }

        public FancyCrystalSkull   (Serial serial) : base(serial) { }

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

    public class HumanCarvingKit    : Item
    {
        [Constructable]
        public HumanCarvingKit   () : base(0x992E)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Human Carving Kit";
        }

        public HumanCarvingKit   (Serial serial) : base(serial) { }

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

	    public class FancyShipWheel    : Item
    {
        [Constructable]
        public FancyShipWheel   () : base(0x6278)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Ship Wheel";
        }

        public FancyShipWheel   (Serial serial) : base(serial) { }

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
	
    public class CupOfSlime    : Item
    {
        [Constructable]
        public CupOfSlime   () : base(0x4CDD)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Cup Of Slime";
        }

        public CupOfSlime   (Serial serial) : base(serial) { }

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

    public class RareWire    : Item
    {
        [Constructable]
        public RareWire   () : base(0x4CDB)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Rare Wire";
        }

        public RareWire   (Serial serial) : base(serial) { }

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

    public class FillerPowder    : Item
    {
        [Constructable]
        public FillerPowder   () : base(0x4CD8)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Filler Powder";
        }

        public FillerPowder   (Serial serial) : base(serial) { }

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

    public class BrassBell    : Item
    {
        [Constructable]
        public BrassBell   () : base(0x4C5C)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Brass Bell";
        }

        public BrassBell   (Serial serial) : base(serial) { }

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

    public class TinCowbell    : Item
    {
        [Constructable]
        public TinCowbell   () : base(0x4C5A)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "TinCowbell";
        }

        public TinCowbell   (Serial serial) : base(serial) { }

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

    public class MasterCello    : Item
    {
        [Constructable]
        public MasterCello   () : base(0x4C3E)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Master Cello";
        }

        public MasterCello   (Serial serial) : base(serial) { }

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

    public class MasterTrumpet    : Item
    {
        [Constructable]
        public MasterTrumpet   () : base(0x4C3D)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Master Trumpet";
        }

        public MasterTrumpet   (Serial serial) : base(serial) { }

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

    public class BlueberryPie    : Food
    {
        [Constructable]
        public BlueberryPie   () : base(0x4C0C)
        {
            Name = "Blueberry Pie";
            Hue = 0x59C;
            Stackable = false;
            Weight = 1.0;
            FillFactor = 10;
        }

        public override bool Eat(Mobile from)
        {
            if (base.Eat(from))
            {
                from.Hits += 10; // Restore some health
                from.CurePoison(from); // Cure minor poisons
                from.SendMessage(0x59B, "The sweet pie soothes your body and mind.");
                return true;
            }
            return false;
        }

        public BlueberryPie   (Serial serial) : base(serial) { }

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

    public class MaxxiaDust    : Item
    {
        [Constructable]
        public MaxxiaDust   () : base(0x4C09)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "MaxxiaDust";
        }

        public MaxxiaDust   (Serial serial) : base(serial) { }

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

    public class FancyHornOfPlenty    : Item
    {
        [Constructable]
        public FancyHornOfPlenty   () : base(0x4BDA)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Horn Of Plenty";
        }

        public FancyHornOfPlenty   (Serial serial) : base(serial) { }

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

    public class PlatinumChip    : Item
    {
        [Constructable]
        public PlatinumChip   () : base(0x4BC6)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Platinum Chip";
        }

        public PlatinumChip   (Serial serial) : base(serial) { }

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

    public class UnluckyDice    : Item
    {
        [Constructable]
        public UnluckyDice   () : base(0x4BB4)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Unlucky Dice";
        }

        public UnluckyDice   (Serial serial) : base(serial) { }

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

    public class LuckyDice    : Item
    {
        [Constructable]
        public LuckyDice   () : base(0x4BBF)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Lucky Dice";
        }

        public LuckyDice   (Serial serial) : base(serial) { }

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

    public class GrandmasSpecialRolls    : Item
    {
        [Constructable]
        public GrandmasSpecialRolls   () : base(0x4BAB)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Grandmas Special Rolls";
        }

        public GrandmasSpecialRolls   (Serial serial) : base(serial) { }

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

    public class JesterSkull    : Item
    {
        [Constructable]
        public JesterSkull   () : base(0x4BA5)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Jester Skull";
        }

        public JesterSkull   (Serial serial) : base(serial) { }

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

    public class PlagueBanner    : Item
    {
        [Constructable]
        public PlagueBanner   () : base(0x4B8D)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Plague Banner";
        }

        public PlagueBanner   (Serial serial) : base(serial) { }

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

    public class NexusShard    : Item
    {
        [Constructable]
        public NexusShard   () : base(0x4B84)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Nexus Shard";
        }

        public NexusShard   (Serial serial) : base(serial) { }

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

    public class UncrackedGeode    : Item
    {
        [Constructable]
        public UncrackedGeode   () : base(0x4B4A)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Uncracked Geode";
        }

        public UncrackedGeode   (Serial serial) : base(serial) { }

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

    public class HorribleMask    : Item
    {
        [Constructable]
        public HorribleMask   () : base(0x4A91)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Horrible Mask";
        }

        public HorribleMask   (Serial serial) : base(serial) { }

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

    public class AtomicRegulator    : Item
    {
        [Constructable]
        public AtomicRegulator   () : base(0x49C2)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Atomic Regulator";
        }

        public AtomicRegulator   (Serial serial) : base(serial) { }

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

    public class FineHoochJug    : Item
    {
        [Constructable]
        public FineHoochJug   () : base(0x495F)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Fine Hooch Jug";
        }

        public FineHoochJug   (Serial serial) : base(serial) { }

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

    public class WelcomeMat    : Item
    {
        [Constructable]
        public WelcomeMat   () : base(0x4790)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Welcome Mat";
        }

        public WelcomeMat   (Serial serial) : base(serial) { }

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

    public class AncientDrum    : Item
    {
        [Constructable]
        public AncientDrum   () : base(0x4581)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Ancient Drum";
        }

        public AncientDrum   (Serial serial) : base(serial) { }

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

    public class AncientRunes    : Item
    {
        [Constructable]
        public AncientRunes   () : base(0x4558)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Ancient Runes";
        }

        public AncientRunes   (Serial serial) : base(serial) { }

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

    public class FancyMirror    : Item
    {
        [Constructable]
        public FancyMirror   () : base(0x4044)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Fancy Mirror";
        }

        public FancyMirror   (Serial serial) : base(serial) { }

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

    public class SilverMirror    : Item
    {
        [Constructable]
        public SilverMirror   () : base(0x403A)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Silver Mirror";
        }

        public SilverMirror   (Serial serial) : base(serial) { }

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

    public class StoneHead    : Item
    {
        [Constructable]
        public StoneHead   () : base(0x3B0F)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Stone Head";
        }

        public StoneHead   (Serial serial) : base(serial) { }

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

    public class SexWhip    : Item
    {
        [Constructable]
        public SexWhip   () : base(0x318A)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Sex Whip";
        }

        public SexWhip   (Serial serial) : base(serial) { }

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

    public class GingerbreadCookie    : Item
    {
        [Constructable]
        public GingerbreadCookie   () : base(0x2BE1)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Gingerbread Cookie";
        }

        public GingerbreadCookie   (Serial serial) : base(serial) { }

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

    public class MillStones    : Item
    {
        [Constructable]
        public MillStones   () : base(0x1888)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Mill Stones";
        }

        public MillStones   (Serial serial) : base(serial) { }

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

    public class SheepCarcass    : Item
    {
        [Constructable]
        public SheepCarcass   () : base(0x1874)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Sheep Carcass";
        }

        public SheepCarcass   (Serial serial) : base(serial) { }

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

    public class FineCopperWire    : Item
    {
        [Constructable]
        public FineCopperWire   () : base(0x1879)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Fine Copper Wire";
        }

        public FineCopperWire   (Serial serial) : base(serial) { }

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

    public class FineGoldWire    : Item
    {
        [Constructable]
        public FineGoldWire   () : base(0x1878)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Fine Gold Wire";
        }

        public FineGoldWire   (Serial serial) : base(serial) { }

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

    public class FineSilverWire    : Item
    {
        [Constructable]
        public FineSilverWire   () : base(0x1877)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Fine Silver Wire";
        }

        public FineSilverWire   (Serial serial) : base(serial) { }

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

    public class FineIronWire    : Item
    {
        [Constructable]
        public FineIronWire   () : base(0x1876)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Fine Iron Wire";
        }

        public FineIronWire   (Serial serial) : base(serial) { }

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

    public class EvilCandle    : Item
    {
        [Constructable]
        public EvilCandle   () : base(0x1854)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Evil Candle";
        }

        public EvilCandle   (Serial serial) : base(serial) { }

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

    public class CompoundF    : Item
    {
        [Constructable]
        public CompoundF   () : base(0x185E)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Compound F";
        }

        public CompoundF   (Serial serial) : base(serial) { }

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

    public class FixedScales    : Item
    {
        [Constructable]
        public FixedScales   () : base(0x1852)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Fixed Scales";
        }

        public FixedScales   (Serial serial) : base(serial) { }

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

    public class SubOil    : Item
    {
        [Constructable]
        public SubOil   () : base(0x1848)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Sub Oil";
        }

        public SubOil   (Serial serial) : base(serial) { }

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

    public class FluxCompound    : Item
    {
        [Constructable]
        public FluxCompound   () : base(0x1844)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Flux Compound";
        }

        public FluxCompound   (Serial serial) : base(serial) { }

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

    public class ReactiveHormones    : Item
    {
        [Constructable]
        public ReactiveHormones   () : base(0x1841)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Reactive Hormones";
        }

        public ReactiveHormones   (Serial serial) : base(serial) { }

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

    public class ElementRegular    : Item
    {
        [Constructable]
        public ElementRegular   () : base(0x183E)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Element Regular";
        }

        public ElementRegular   (Serial serial) : base(serial) { }

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

    public class DistilledEssence    : Item
    {
        [Constructable]
        public DistilledEssence   () : base(0x183B)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Distilled Essence";
        }

        public DistilledEssence   (Serial serial) : base(serial) { }

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

    public class DistillationFlask    : Item
    {
        [Constructable]
        public DistillationFlask   () : base(0x1836)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Distillation Flask";
        }

        public DistillationFlask   (Serial serial) : base(serial) { }

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

    public class HydroxFluid    : Item
    {
        [Constructable]
        public HydroxFluid   () : base(0x1831)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Hydrox Fluid";
        }

        public HydroxFluid   (Serial serial) : base(serial) { }

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

    public class FleshLight    : Item
    {
        [Constructable]
        public FleshLight   () : base(0x1724)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Flesh Light";
        }

        public FleshLight   (Serial serial) : base(serial) { }

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

    public class SerpantCrest    : Item
    {
        [Constructable]
        public SerpantCrest   () : base(0x1514)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Serpant Crest";
        }

        public SerpantCrest   (Serial serial) : base(serial) { }

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

    public class BakingBoard    : Item
    {
        [Constructable]
        public BakingBoard   () : base(0x14E9)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Baking Board";
        }

        public BakingBoard   (Serial serial) : base(serial) { }

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

    public class GamerJelly    : Item
    {
        [Constructable]
        public GamerJelly   () : base(0x142B)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Gamer Jelly";
        }

        public GamerJelly   (Serial serial) : base(serial) { }

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

    public class MahJongTile    : Item
    {
        [Constructable]
        public MahJongTile   () : base(0x1422)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "MahJong Tile";
        }

        public MahJongTile   (Serial serial) : base(serial) { }

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

    public class ScentedCandle    : Item
    {
        [Constructable]
        public ScentedCandle   () : base(0x1430)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Scented Candle";
        }

        public ScentedCandle   (Serial serial) : base(serial) { }

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

    public class DeckOfMagicCards    : Item
    {
        [Constructable]
        public DeckOfMagicCards   () : base(0x12AB)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Magic Cards";
        }

        public DeckOfMagicCards   (Serial serial) : base(serial) { }

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

    public class EnchantedRose    : Item
    {
        [Constructable]
        public EnchantedRose   () : base(0x0EB0)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Enchanted Rose";
        }

        public EnchantedRose   (Serial serial) : base(serial) { }

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

    public class DressForm    : Item
    {
        [Constructable]
        public DressForm   () : base(0x0EC6)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Dress Form";
        }

        public DressForm   (Serial serial) : base(serial) { }

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

    public class BioSamples    : Item
    {
        [Constructable]
        public BioSamples   () : base(0x0E4B)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Bio Samples";
        }

        public BioSamples   (Serial serial) : base(serial) { }

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

    public class FairyOil    : Item
    {
        [Constructable]
        public FairyOil   () : base(0x0E4B)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Fairy Oil";
        }

        public FairyOil   (Serial serial) : base(serial) { }

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

    public class WoodAlchohol    : Item
    {
        [Constructable]
        public WoodAlchohol   () : base(0x0E2B)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Wood Alchohol";
        }

        public WoodAlchohol   (Serial serial) : base(serial) { }

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

    public class EssenceOfToad    : Item
    {
        [Constructable]
        public EssenceOfToad   () : base(0x0E2A)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Essence Of Toad";
        }

        public EssenceOfToad   (Serial serial) : base(serial) { }

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
	
    public class Jet    : Item
    {
        [Constructable]
        public Jet   () : base(0x0E28)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Jet";
        }

        public Jet   (Serial serial) : base(serial) { }

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
	
    public class Steroids    : Item
    {
        [Constructable]
        public Steroids   () : base(0x0E29)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Steroids";
        }

        public Steroids   (Serial serial) : base(serial) { }

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
	
    public class BottledPlague    : Item
    {
        [Constructable]
        public BottledPlague   () : base(0x0E27)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Bottled Plague";
        }

        public BottledPlague   (Serial serial) : base(serial) { }

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

    public class PineResin    : Item
    {
        [Constructable]
        public PineResin   () : base(0x0E26)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Pine Resin";
        }

        public PineResin   (Serial serial) : base(serial) { }

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

    public class RareGrease    : Item
    {
        [Constructable]
        public RareGrease   () : base(0x0E25)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Rare Grease";
        }

        public RareGrease   (Serial serial) : base(serial) { }

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

    public class WigStand    : Item
    {
        [Constructable]
        public WigStand   () : base(0x0E06)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Wig Stand";
        }

        public WigStand   (Serial serial) : base(serial) { }

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

    public class GrandmasKnitting    : Item
    {
        [Constructable]
        public GrandmasKnitting   () : base(0x0DF6)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Grandmas Knitting";
        }

        public GrandmasKnitting   (Serial serial) : base(serial) { }

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

    public class FunMushroom    : Item
    {
        [Constructable]
        public FunMushroom   () : base(0x0D19)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Fun Mushroom";
        }

        public FunMushroom   (Serial serial) : base(serial) { }

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

    public class BeeHive    : Item
    {
        [Constructable]
        public BeeHive   () : base(0x091A)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Bee Hive";
        }

        public BeeHive   (Serial serial) : base(serial) { }

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

    public class OrganicHeart    : Item
    {
        [Constructable]
        public OrganicHeart   () : base(0x024B)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Organic Heart";
        }

        public OrganicHeart   (Serial serial) : base(serial) { }

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

    public class WorldShard    : Item
    {
        [Constructable]
        public WorldShard   () : base(0x023B)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "World Shard";
        }

        public WorldShard   (Serial serial) : base(serial) { }

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

    public class RopeSpindle    : Item
    {
        [Constructable]
        public RopeSpindle   () : base(0x020D)
        {
            Weight = 1.0;
            Hue = 0;
            Name = "Rope Spindle";
        }

        public RopeSpindle   (Serial serial) : base(serial) { }

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
