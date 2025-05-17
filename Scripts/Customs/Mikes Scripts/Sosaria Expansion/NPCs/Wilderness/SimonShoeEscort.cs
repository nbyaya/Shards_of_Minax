using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{

	public class SimonShoeQuest : BaseQuest
	{
		public override object Title { get { return "If The Shoe Fits..."; } }

		public override object Description
		{
			get
			{
				return 
					"Oh thank the stars you’re here! I’m Simon Shoe, cobbler by trade... and, uh, accidental curse-bearer by hobby? I ventured too far from Trinsic Inn chasing rumors of rare leather, and now my shoes won’t let me rest! They want to walk, but not back to safety, oh no. Please, guide me back to Trinsic Inn in Moon before these cursed soles drag me into the Void!";
			}
		}

		public override object Refuse { get { return "You don’t understand, these shoes—they whisper things. I must get home before they walk me into the dark!"; } }
		public override object Uncomplete { get { return "Please hurry, my shoes are getting restless again..."; } }

		public SimonShoeQuest() : base()
		{
			AddObjective(new EscortObjective("Trinsic Inn"));
			AddReward(new BaseReward(typeof(MaxxiaScroll), "Shoemaker's Charm"));
			AddReward(new BaseReward(typeof(MaxxiaScroll), "Animated Sandal Pet"));
		}

		public override void GiveRewards()
		{
			base.GiveRewards();
			Owner.SendMessage("Thank you, kind friend! I think... I think they’re quiet now. Take this charm, may your steps always be lighter than mine have been!", null, 0x59B);
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

	public class SimonShoeEscort : BaseEscort
	{
		public override Type[] Quests { get { return new Type[] { typeof(SimonShoeQuest) }; } }
		public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBCobbler());
		}

		private DateTime m_NextTalkTime;

		[Constructable]
		public SimonShoeEscort() : base()
		{
			Name = "Simon Shoe";
			Title = "the Lost Shoemaker";
			NameHue = 0x83F;
		}

		public SimonShoeEscort(Serial serial) : base(serial) { }

		public override void InitBody()
		{
			InitStats(50, 50, 25);
			Female = false;
			CantWalk = false;
			Race = Race.Human;
			Hue = 0x8401;
			HairItemID = 0x2047;
			HairHue = 1210;
		}

		public override void InitOutfit()
		{
			AddItem(new FormalShirt() { Hue = 0x47E });
			AddItem(new ElvenPants() { Hue = 0x482 });
			AddItem(new HalfApron() { Hue = 0x59C });
			AddItem(new FeatheredHat() { Hue = 0x5BE });
			AddItem(new Sandals() { Hue = 0x48F });
			AddItem(new CarpentersHammer());
		}

		public override void OnThink()
		{
			base.OnThink();

			if (DateTime.Now >= m_NextTalkTime && this.Controlled)
			{
				// Random chance to speak every 15-30 seconds
				if (Utility.RandomDouble() < 0.1) // 10% chance to talk each tick
				{
					string[] lines = new string[]
					{
						"*Simon glances down at his shoes, muttering* 'Quiet now, Heelie, I said not now...'",
						"*Simon pauses and listens* 'Did you hear that? They’re... they’re whispering again!'",
						"'Please tell me you know the way... I think we’ve walked this path before... in my dreams.'",
						"*Simon nervously taps his foot* 'Don’t trust the cobblestones... some of them bite.'",
						"*He chuckles weakly* 'They said cobblers walk a lonely road... they didn’t mean this lonely!'",
						"*Simon suddenly stops* 'One more step and I swear I’ll sew your mouth shut!'",
						"'Oh Trinsic Inn, warm hearth, safe floors... hold on, I’m almost home.'"
					};

					Say(lines[Utility.Random(lines.Length)]);
					m_NextTalkTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30));
				}
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
			writer.Write(m_NextTalkTime);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			m_NextTalkTime = reader.ReadDateTime();
		}
	}
}
