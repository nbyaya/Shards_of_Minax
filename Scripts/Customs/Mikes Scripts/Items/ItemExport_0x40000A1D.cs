using System;
using Server;
using Server.Items;

namespace Server.Scripts.Items
{
    public class CustomDagger : Dagger
    {
        [Constructable]
        public CustomDagger()
        {
            // PrimaryAbility = "Server.Items.ShadowStrike";
            // SecondaryAbility = "Server.Items.InfectiousStrike";
            // AosStrengthReq = 10;
            // AosMinDamage = 10;
            // AosMaxDamage = 12;
            // AosSpeed = 56;
            // MlSpeed = 2;
            // OldStrengthReq = 1;
            // OldMinDamage = 3;
            // OldMaxDamage = 15;
            // OldSpeed = 55;
            // InitMinHits = 31;
            // InitMaxHits = 40;
            // DefSkill = "Fencing";
            // DefType = "Piercing";
            // DefAnimation = "Pierce1H";
            // DefHitSound = 571;
            // DefMissSound = 568;
            // UsesRemaining = 150;
            // ShowUsesRemaining = false;
            // IsVvVItem = false;
            // DefMaxRange = 1;
            // AosDexterityReq = 0;
            // AosIntelligenceReq = 0;
            // AosMaxRange = 1;
            // AosHitSound = 571;
            // AosMissSound = 568;
            // AosSkill = "Fencing";
            // AosType = "Piercing";
            // AosAnimation = "Pierce1H";
            // OldDexterityReq = 0;
            // OldIntelligenceReq = 0;
            // OldMaxRange = 1;
            // OldHitSound = 571;
            // OldMissSound = 568;
            // OldSkill = "Fencing";
            // OldType = "Piercing";
            // OldAnimation = "Pierce1H";
            // CanFortify = true;
            // CanRepair = true;
            // CanAlter = true;
            // PhysicalResistance = 0;
            // FireResistance = 0;
            // ColdResistance = 0;
            // PoisonResistance = 0;
            // EnergyResistance = 0;
            // AccuracySkill = "Tactics";
            // DefaultWeight = 1;
            // Attributes = "...";
            // WeaponAttributes = "...";
            // SkillBonuses = "...";
            // AosElementDamages = "...";
            // AbsorptionAttributes = "...";
            // NegativeAttributes = "...";
            // ExtendedWeaponAttributes = "...";
            // Identified = false;
            // HitPoints = 36;
            // MaxHitPoints = 36;
            // PoisonCharges = 0;
            // Quality = "Normal";
            // Slayer = "None";
            // Slayer2 = "None";
            // Slayer3 = "None";
            // Resource = "Iron";
            // DamageLevel = "Regular";
            // DurabilityLevel = "Regular";
            // PlayerConstructed = false;
            // MaxRange = 1;
            // Animation = "Pierce1H";
            // Type = "Piercing";
            // Skill = "Fencing";
            // HitSound = 571;
            // MissSound = 568;
            // MinDamage = 10;
            // MaxDamage = 12;
            // Speed = 2;
            // StrRequirement = 10;
            // DexRequirement = 0;
            // IntRequirement = 0;
            // AccuracyLevel = "Regular";
            // LastParryChance = 0;
            // TimesImbued = 0;
            // IsImbued = false;
            // DImodded = false;
            // BaseResists = "System.Int32[]";
            // SearingWeapon = false;
            // ItemPower = "None";
            // ReforgedPrefix = "None";
            // ReforgedSuffix = "None";
            // CanBeWornByGargoyles = false;
            // UseSkillMod = false;
            // InDoubleStrike = false;
            // ProcessingMultipleHits = false;
            // EndDualWield = false;
            // BlockHitEffects = false;
            // NextSelfRepair = "0001-01-01 12:00:00 AM";
            // VirtualDamageBonus = 0;
            // Hue = 0;
            // ArtifactRarity = 0;
            // DisplayWeight = true;
            // SetID = "None";
            // Pieces = 0;
            // BardMasteryBonus = false;
            // IsSetItem = false;
            // SetHue = 0;
            // SetEquipped = false;
            // LastEquipped = false;
            // SetAttributes = "...";
            // SetSkillBonuses = "...";
            // SetSelfRepair = 0;
            // SetPhysicalBonus = 0;
            // SetFireBonus = 0;
            // SetColdBonus = 0;
            // SetPoisonBonus = 0;
            // SetEnergyBonus = 0;
            // Altered = false;
            // Modules = "System.Collections.Generic.List`1[CustomsFramework.BaseModule]";
            // TempFlags = 0;
            // SavedFlags = 0;
            // GridLocation = 3;
            // HonestyItem = false;
            // Deleted = false;
            // LootType = "Regular";
            // IsArtifact = false;
            // DecayMultiplier = 1;
            // DefaultDecaySetting = true;
            // DecayTime = "01:00:00";
            // Decays = true;
            // TimeToDecay = "00:52:31.8450000";
            // LastMoved = "2024-08-25 11:02:40 AM";
            // StackIgnoreItemID = false;
            // StackIgnoreHue = false;
            // StackIgnoreName = false;
            // Stackable = false;
            // RemovePacket = "Server.Network.RemoveItem";
            // OPLPacket = "Server.OPLInfo";
            // PropertyList = "Server.ObjectPropertyList";
            // WorldPacket = "Server.Network.WorldItem";
            // WorldPacketSA = "Server.Network.WorldItemSA";
            // WorldPacketHS = "Server.Network.WorldItemHS";
            // Visible = true;
            // Movable = true;
            // ForceShowProperties = false;
            // HandlesOnMovement = false;
            // IsLockedDown = false;
            // IsSecure = false;
            // IsVirtualItem = false;
            // LabelNumber = 1023922;
            // TotalGold = 0;
            // TotalItems = 0;
            // TotalWeight = 0;
            // Weight = 1;
            // PileWeight = 1;
            // HuedItemID = 3922;
            // HiddenQuestItemHue = false;
            // QuestItemHue = 1258;
            // Nontransferable = false;
            // Layer = "FirstValid";
            // Items = "System.Collections.Generic.List`1[Server.Item]";
            // RootParent = "0x74 "Selectron"";
            // NoMoveHS = false;
            // ParentEntity = "0x40000A19 "Backpack"";
            // RootParentEntity = "0x74 "Selectron"";
            // Location = "(72, 69, 0)";
            // X = 72;
            // Y = 69;
            // Z = 0;
            // ItemID = 3922;
            // Light = "Empty";
            // Direction = "North";
            // Amount = 1;
            // HandlesOnSpeech = false;
            // BlocksFit = false;
            // InSecureTrade = false;
            // ItemData = "Server.ItemData";
            // CanTarget = true;
            // DisplayLootType = true;
            // QuestItem = false;
            // Insured = false;
            // PayedInsurance = false;
        }

        public CustomDagger(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
