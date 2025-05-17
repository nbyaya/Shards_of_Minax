using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{

	public class ConcreteGargoyleQuest : BaseQuest
	{
		public override bool DoneOnce { get { return true; } }

		public override object Title { get { return "Concrete Gargoyle"; } }

		public override object Description
		{
			get
			{
				return
					"Losto Emberwraith, a spectral figure flickering near the steaming baths of Devil Guard, gazes at you with eyes that seem to see beyond the present.\n\n" +
					"“It took something precious... a key, ancient and binding. Sealed for centuries in Vault 17, now lost to the Mines. This creature—formed of stone and rage—walks the depths with the stolen key lodged in its jagged heart.”\n\n" +
					"“I cannot banish it alone. But I see it. Each night. Fused to the rock, blocky limbs grinding through the earth. You must slay it. Reclaim the key. Or the Vault will never open again.”";
			}
		}

		public override object Refuse
		{
			get { return "Then may the key lie lost, and the Vault remain silent in the dark."; }
		}

		public override object Uncomplete
		{
			get { return "The gargoyle still walks? I hear it, even now... crushing stone with each cursed step."; }
		}

		public override object Complete
		{
			get
			{
				return
					"The Vault's whisper fades... you've done well.\n\n" +
					"The key is safe, and the stone beast silenced. Take this robe—it was worn by the Bloomguard, protectors of life and light. You have earned it.";
			}
		}

		public ConcreteGargoyleQuest() : base()
		{
			AddObjective(new SlayObjective(typeof(MinecraftGargolye), "Minecraft Gargoyle", 1));
			AddReward(new BaseReward(typeof(GownOfTheBloomguard), 1, "Gown of the Bloomguard"));
		}

		public override void OnCompleted()
		{
			Owner.SendMessage(0x23, "You've completed 'Concrete Gargoyle'!");
			Owner.PlaySound(CompleteSound);
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

	public class LostoEmberwraith : MondainQuester
	{
		public override Type[] Quests { get { return new Type[] { typeof(ConcreteGargoyleQuest) }; } }

		public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBProvisioner());
		}

		[Constructable]
		public LostoEmberwraith()
			: base("the Haunted Guide", "Losto Emberwraith")
		{
		}

		public LostoEmberwraith(Serial serial) : base(serial) { }

		public override void InitBody()
		{
			InitStats(60, 70, 50);
			Female = false;
			Body = 0x190; // Male
			Race = Race.Human;

			Hue = 1150; // Ghostly pale hue
			HairItemID = 0x203C; // Long hair
			HairHue = 1153; // White-blue spectral
			FacialHairItemID = 0x204B; // Long beard
			FacialHairHue = 1153;
		}

		public override void InitOutfit()
		{
			AddItem(new MonkRobe() { Hue = 2101, Name = "Robes of the Lost Flame" }); // Deep spectral blue
			AddItem(new HoodedShroudOfShadows() { Hue = 1150, Name = "Wraith’s Veil" });
			AddItem(new Sandals() { Hue = 1153, Name = "Silent Steps" });
			AddItem(new BodySash() { Hue = 2105, Name = "Sash of Binding Visions" });

			AddItem(new MysticStaff() { Hue = 2106, Name = "Echo of Emberlight" });

			Backpack backpack = new Backpack();
			backpack.Hue = 1150;
			backpack.Name = "Spectral Satchel";
			AddItem(backpack);
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